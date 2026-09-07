using Microsoft.AspNetCore.Mvc;
using SupportAuditSystem.Services;

namespace SupportAuditSystem.Controllers
{
    [ApiController]
    [Route("api/checklist")]
    public class ChecklistApiController : ControllerBase
    {
        private readonly ChecklistService _checklists;
        private readonly PdfExportService _pdf;
        private readonly UserService _users;
        private readonly ILogger<ChecklistApiController> _log;

        public ChecklistApiController(ChecklistService checklists, PdfExportService pdf, UserService users, ILogger<ChecklistApiController> log)
        {
            _checklists = checklists;
            _pdf = pdf;
            _users = users;
            _log = log;
        }

        public record ResponseDto(int TaskItemId, string? Status, string? Notes);
        public record CheckpointDto(int CheckpointResponseId, bool Ticked);
        public record HeaderDto(string? AuditorNames, string? Location);

        [HttpPost("{id:int}/response")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveResponse(int id, [FromBody] ResponseDto dto)
        {
            var r = await _checklists.SaveResponseAsync(id, dto.TaskItemId, dto.Status, dto.Notes, _users.GetCurrentUser());
            return r.Ok ? Ok(new { ok = true }) : BadRequest(new { ok = false, error = r.Error });
        }

        [HttpPost("{id:int}/checkpoint")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCheckpoint(int id, [FromBody] CheckpointDto dto)
        {
            var r = await _checklists.SaveCheckpointAsync(id, dto.CheckpointResponseId, dto.Ticked, _users.GetCurrentUser());
            if (!r.Ok) return BadRequest(new { ok = false, error = r.Error });
            return Ok(new { ok = true, tickedAt = dto.Ticked ? DateTime.Now.ToString("HH:mm") : "" });
        }

        [HttpPost("{id:int}/header")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHeader(int id, [FromBody] HeaderDto dto)
        {
            var r = await _checklists.SaveHeaderAsync(id, dto.AuditorNames, dto.Location, _users.GetCurrentUser());
            return r.Ok ? Ok(new { ok = true }) : BadRequest(new { ok = false, error = r.Error });
        }

        [HttpPost("{id:int}/complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var r = await _checklists.CompleteAsync(id, _users.GetCurrentUser());
            if (r.Ok) return Ok(new { ok = true });
            return BadRequest(new { ok = false, error = r.Error, missingNotes = r.MissingNotes });
        }

        [HttpGet("{id:int}/pdf")]
        public async Task<IActionResult> Pdf(int id)
        {
            var sub = await _checklists.GetForEntryAsync(id);
            if (sub == null) return NotFound();
            // Ensure department name is available for the PDF header.
            try
            {
                var bytes = _pdf.GenerateChecklist(sub);
                var area = (sub.Area?.Name ?? "area").Replace(" ", "_");
                var shift = (sub.Shift?.Name ?? "shift").Replace(" ", "_");
                var filename = $"SHEF014_{sub.ChecklistDate:yyyyMMdd}_{area}_{shift}.pdf";
                return File(bytes, "application/pdf", filename);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Checklist PDF failed for id {Id}", id);
                return new ContentResult
                {
                    StatusCode = 500,
                    ContentType = "text/plain",
                    Content = "PDF generation failed. Please try again or contact support.",
                };
            }
        }
    }
}
