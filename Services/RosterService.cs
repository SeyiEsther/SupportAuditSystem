using Microsoft.EntityFrameworkCore;
using SupportAuditSystem.Data;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Services
{
    // Named-person rosters for sign-off pickers (starting with HOD). Mirrors the
    // Production Audit System's PickerPerson/PersonListService pattern: a fixed
    // roster "kind" the app understands structurally, but the people in it are
    // fully admin-editable data — add-only sync so a default name added later
    // still appears without wiping anything an admin already changed.
    public class RosterService
    {
        private readonly AppDbContext _db;

        public RosterService(AppDbContext db) => _db = db;

        public Task<List<RosterPerson>> GetAsync(string listKind) =>
            _db.RosterPeople
                .Where(p => p.ListKind == listKind && p.IsActive)
                .OrderBy(p => p.SortOrder).ThenBy(p => p.Name)
                .ToListAsync();

        public Task<List<RosterPerson>> GetAllAsync(string listKind) =>
            _db.RosterPeople
                .Where(p => p.ListKind == listKind)
                .OrderBy(p => p.SortOrder).ThenBy(p => p.Name)
                .ToListAsync();

        public async Task<bool> AddAsync(string listKind, string name)
        {
            var trimmed = (name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(trimmed)) return false;

            var exists = await _db.RosterPeople.AnyAsync(p =>
                p.ListKind == listKind && p.Name.ToLower() == trimmed.ToLower());
            if (exists) return false;

            var maxOrder = await _db.RosterPeople
                .Where(p => p.ListKind == listKind)
                .Select(p => (int?)p.SortOrder).MaxAsync() ?? 0;

            _db.RosterPeople.Add(new RosterPerson { ListKind = listKind, Name = trimmed, SortOrder = maxOrder + 1 });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var p = await _db.RosterPeople.FindAsync(id);
            if (p == null) return false;
            _db.RosterPeople.Remove(p);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
