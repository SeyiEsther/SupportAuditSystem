using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAuditSystem.Migrations
{
    /// <summary>
    /// Seeds Dispatch's Warehouse Audit — transcribed from the supplied spreadsheet.
    /// Dispatch had no areas or shifts yet; this creates a single "Warehouse" area and
    /// the site's 1st/2nd/3rd/Continental nights shift pattern (mirroring Stores, since
    /// no shift-specific content was given and it's the same facility), then seeds the
    /// same 24-item audit identically across all four.
    ///
    /// Category, cadence, the accountable role and the escalation target/window are
    /// carried as their OWN structured columns on TaskItem (not folded into the task
    /// text) so the "who's doing what" column from the source is real, queryable data —
    /// rendered as badges on the entry screen and in the PDF. Hourly-cadence items are
    /// time-boxed with generic Hour 1-8 checkpoints (no specific clock times were given).
    ///
    /// Also seeds the HOD roster used for sign-off with the SAME names as the Production
    /// Audit System (TL)'s HOD list, so HOD sign-off here draws on the identical roster.
    /// All of this is admin-editable data — nothing here is hard-coded elsewhere.
    /// </summary>
    public partial class SeedDispatchWarehouseAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "DepartmentId", "Name", "DefaultLocation", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 3, 2, "Warehouse", "Warehouse", 1, true },
                });
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "DepartmentId", "Name", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 5, 2, "1st", 1, true },
                    { 6, 2, "2nd", 2, true },
                    { 7, 2, "3rd", 3, true },
                    { 8, 2, "Continental nights", 4, true },
                });
            migrationBuilder.InsertData(
                table: "TaskLists",
                columns: new[] { "Id", "AreaId", "ShiftId", "Version", "IsCurrent", "HealthRepsReminder", "CreatedAt", "CreatedBy" },
                values: new object[,]
                {
                    { 9, 3, 5, 1, true, null, new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 10, 3, 6, 1, true, null, new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 11, 3, 7, 1, true, null, new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 12, 3, 8, 1, true, null, new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                });
            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "TaskListId", "Text", "SortOrder", "IsTimeBoxed", "Category", "Cadence", "ResponsibleRole", "EscalateToRole", "EscalationWindow" },
                values: new object[,]
                {
                    { 53, 9, "Walkways, fire exits and emergency routes are clear", 0, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 54, 9, "PPE requirements are being followed", 1, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 55, 9, "No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)", 2, true, "H&S", "Hourly", "Senior Operator", null, "immediately" },
                    { 56, 9, "Loading bays, dock levellers and vehicle Security is safe (Rite Height)", 3, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 57, 9, "Correct product, quantity and destination are being picked (Bay Sheets)", 4, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 58, 9, "Labels, paperwork and scanning are accurate (Check Scanner V Loading)", 5, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 59, 9, "Picking and despatch activity is on plan", 6, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 60, 9, "Bottlenecks, downtime and waiting vehicles are controlled", 7, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 61, 9, "Pre-use equipment checks are completed and defects reported (Check Booklets)", 8, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 62, 9, "Spill kits, first-aid and fire points are accessible", 9, false, "H&S", "Per Shift", "Senior Operator", null, "immediately" },
                    { 63, 9, "Pedestrian and MHE segregation controls are effective", 10, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 64, 9, "Packaging standards and load security are acceptable", 11, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 65, 9, "Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card", 12, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 66, 9, "FIFO/stock rotation requirements are followed where applicable (New)", 13, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 67, 9, "Shift plan, priorities and cut-off times were communicated (Shift Start Up)", 14, false, "Performance", "Per Shift", "HOD", null, "within the same shift" },
                    { 68, 9, "Labour and equipment resources are adequate for the plan", 15, false, "Performance", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 69, 9, "Shift KPIs, backlog and carry-over work were reviewed (RPS)", 16, false, "Performance", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 70, 9, "Team brief and safety message were completed ( Start of Each Shift Team Brief)", 17, false, "Morale", "Per Shift", "HOD", null, "within the same shift" },
                    { 71, 9, "Team concerns, support needs and training gaps were discussed", 18, false, "Morale", "Per Shift", "HOD", null, "by the next shift" },
                    { 72, 9, "Good performance and positive behaviours were recognised", 19, false, "Morale", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 73, 9, "General housekeeping and waste controls meet standard", 20, false, "H&S", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 74, 9, "A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)", 21, false, "Quality", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 75, 9, "Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)", 22, false, "Performance", "Daily", "Senior Operator", null, "within the same day" },
                    { 76, 9, "Absence, overtime, workload and welfare concerns were reviewed", 23, false, "Morale", "Daily", "HOD", null, "within the same day" },
                    { 77, 10, "Walkways, fire exits and emergency routes are clear", 0, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 78, 10, "PPE requirements are being followed", 1, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 79, 10, "No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)", 2, true, "H&S", "Hourly", "Senior Operator", null, "immediately" },
                    { 80, 10, "Loading bays, dock levellers and vehicle Security is safe (Rite Height)", 3, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 81, 10, "Correct product, quantity and destination are being picked (Bay Sheets)", 4, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 82, 10, "Labels, paperwork and scanning are accurate (Check Scanner V Loading)", 5, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 83, 10, "Picking and despatch activity is on plan", 6, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 84, 10, "Bottlenecks, downtime and waiting vehicles are controlled", 7, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 85, 10, "Pre-use equipment checks are completed and defects reported (Check Booklets)", 8, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 86, 10, "Spill kits, first-aid and fire points are accessible", 9, false, "H&S", "Per Shift", "Senior Operator", null, "immediately" },
                    { 87, 10, "Pedestrian and MHE segregation controls are effective", 10, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 88, 10, "Packaging standards and load security are acceptable", 11, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 89, 10, "Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card", 12, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 90, 10, "FIFO/stock rotation requirements are followed where applicable (New)", 13, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 91, 10, "Shift plan, priorities and cut-off times were communicated (Shift Start Up)", 14, false, "Performance", "Per Shift", "HOD", null, "within the same shift" },
                    { 92, 10, "Labour and equipment resources are adequate for the plan", 15, false, "Performance", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 93, 10, "Shift KPIs, backlog and carry-over work were reviewed (RPS)", 16, false, "Performance", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 94, 10, "Team brief and safety message were completed ( Start of Each Shift Team Brief)", 17, false, "Morale", "Per Shift", "HOD", null, "within the same shift" },
                    { 95, 10, "Team concerns, support needs and training gaps were discussed", 18, false, "Morale", "Per Shift", "HOD", null, "by the next shift" },
                    { 96, 10, "Good performance and positive behaviours were recognised", 19, false, "Morale", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 97, 10, "General housekeeping and waste controls meet standard", 20, false, "H&S", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 98, 10, "A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)", 21, false, "Quality", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 99, 10, "Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)", 22, false, "Performance", "Daily", "Senior Operator", null, "within the same day" },
                    { 100, 10, "Absence, overtime, workload and welfare concerns were reviewed", 23, false, "Morale", "Daily", "HOD", null, "within the same day" },
                    { 101, 11, "Walkways, fire exits and emergency routes are clear", 0, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 102, 11, "PPE requirements are being followed", 1, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 103, 11, "No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)", 2, true, "H&S", "Hourly", "Senior Operator", null, "immediately" },
                    { 104, 11, "Loading bays, dock levellers and vehicle Security is safe (Rite Height)", 3, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 105, 11, "Correct product, quantity and destination are being picked (Bay Sheets)", 4, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 106, 11, "Labels, paperwork and scanning are accurate (Check Scanner V Loading)", 5, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 107, 11, "Picking and despatch activity is on plan", 6, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 108, 11, "Bottlenecks, downtime and waiting vehicles are controlled", 7, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 109, 11, "Pre-use equipment checks are completed and defects reported (Check Booklets)", 8, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 110, 11, "Spill kits, first-aid and fire points are accessible", 9, false, "H&S", "Per Shift", "Senior Operator", null, "immediately" },
                    { 111, 11, "Pedestrian and MHE segregation controls are effective", 10, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 112, 11, "Packaging standards and load security are acceptable", 11, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 113, 11, "Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card", 12, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 114, 11, "FIFO/stock rotation requirements are followed where applicable (New)", 13, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 115, 11, "Shift plan, priorities and cut-off times were communicated (Shift Start Up)", 14, false, "Performance", "Per Shift", "HOD", null, "within the same shift" },
                    { 116, 11, "Labour and equipment resources are adequate for the plan", 15, false, "Performance", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 117, 11, "Shift KPIs, backlog and carry-over work were reviewed (RPS)", 16, false, "Performance", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 118, 11, "Team brief and safety message were completed ( Start of Each Shift Team Brief)", 17, false, "Morale", "Per Shift", "HOD", null, "within the same shift" },
                    { 119, 11, "Team concerns, support needs and training gaps were discussed", 18, false, "Morale", "Per Shift", "HOD", null, "by the next shift" },
                    { 120, 11, "Good performance and positive behaviours were recognised", 19, false, "Morale", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 121, 11, "General housekeeping and waste controls meet standard", 20, false, "H&S", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 122, 11, "A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)", 21, false, "Quality", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 123, 11, "Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)", 22, false, "Performance", "Daily", "Senior Operator", null, "within the same day" },
                    { 124, 11, "Absence, overtime, workload and welfare concerns were reviewed", 23, false, "Morale", "Daily", "HOD", null, "within the same day" },
                    { 125, 12, "Walkways, fire exits and emergency routes are clear", 0, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 126, 12, "PPE requirements are being followed", 1, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 127, 12, "No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)", 2, true, "H&S", "Hourly", "Senior Operator", null, "immediately" },
                    { 128, 12, "Loading bays, dock levellers and vehicle Security is safe (Rite Height)", 3, true, "H&S", "Hourly", "Senior Operator", "HOD", "immediately" },
                    { 129, 12, "Correct product, quantity and destination are being picked (Bay Sheets)", 4, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 130, 12, "Labels, paperwork and scanning are accurate (Check Scanner V Loading)", 5, true, "Quality", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 131, 12, "Picking and despatch activity is on plan", 6, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 132, 12, "Bottlenecks, downtime and waiting vehicles are controlled", 7, true, "Performance", "Hourly", "Senior Operator", null, "within the same shift" },
                    { 133, 12, "Pre-use equipment checks are completed and defects reported (Check Booklets)", 8, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 134, 12, "Spill kits, first-aid and fire points are accessible", 9, false, "H&S", "Per Shift", "Senior Operator", null, "immediately" },
                    { 135, 12, "Pedestrian and MHE segregation controls are effective", 10, false, "H&S", "Per Shift", "Senior Operator", "HOD", "immediately" },
                    { 136, 12, "Packaging standards and load security are acceptable", 11, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 137, 12, "Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card", 12, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 138, 12, "FIFO/stock rotation requirements are followed where applicable (New)", 13, false, "Quality", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 139, 12, "Shift plan, priorities and cut-off times were communicated (Shift Start Up)", 14, false, "Performance", "Per Shift", "HOD", null, "within the same shift" },
                    { 140, 12, "Labour and equipment resources are adequate for the plan", 15, false, "Performance", "Per Shift", "Senior Operator", null, "within the same shift" },
                    { 141, 12, "Shift KPIs, backlog and carry-over work were reviewed (RPS)", 16, false, "Performance", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 142, 12, "Team brief and safety message were completed ( Start of Each Shift Team Brief)", 17, false, "Morale", "Per Shift", "HOD", null, "within the same shift" },
                    { 143, 12, "Team concerns, support needs and training gaps were discussed", 18, false, "Morale", "Per Shift", "HOD", null, "by the next shift" },
                    { 144, 12, "Good performance and positive behaviours were recognised", 19, false, "Morale", "Per Shift", "Senior Operator", "HOD", "by the next shift" },
                    { 145, 12, "General housekeeping and waste controls meet standard", 20, false, "H&S", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 146, 12, "A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)", 21, false, "Quality", "Daily", "Senior Operator", "HOD", "within the same day" },
                    { 147, 12, "Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)", 22, false, "Performance", "Daily", "Senior Operator", null, "within the same day" },
                    { 148, 12, "Absence, overtime, workload and welfare concerns were reviewed", 23, false, "Morale", "Daily", "HOD", null, "within the same day" },
                });
            migrationBuilder.InsertData(
                table: "TaskCheckpoints",
                columns: new[] { "Id", "TaskItemId", "Label", "SortOrder" },
                values: new object[,]
                {
                    { 17, 53, "Hour 1", 0 },
                    { 18, 53, "Hour 2", 1 },
                    { 19, 53, "Hour 3", 2 },
                    { 20, 53, "Hour 4", 3 },
                    { 21, 53, "Hour 5", 4 },
                    { 22, 53, "Hour 6", 5 },
                    { 23, 53, "Hour 7", 6 },
                    { 24, 53, "Hour 8", 7 },
                    { 25, 54, "Hour 1", 0 },
                    { 26, 54, "Hour 2", 1 },
                    { 27, 54, "Hour 3", 2 },
                    { 28, 54, "Hour 4", 3 },
                    { 29, 54, "Hour 5", 4 },
                    { 30, 54, "Hour 6", 5 },
                    { 31, 54, "Hour 7", 6 },
                    { 32, 54, "Hour 8", 7 },
                    { 33, 55, "Hour 1", 0 },
                    { 34, 55, "Hour 2", 1 },
                    { 35, 55, "Hour 3", 2 },
                    { 36, 55, "Hour 4", 3 },
                    { 37, 55, "Hour 5", 4 },
                    { 38, 55, "Hour 6", 5 },
                    { 39, 55, "Hour 7", 6 },
                    { 40, 55, "Hour 8", 7 },
                    { 41, 56, "Hour 1", 0 },
                    { 42, 56, "Hour 2", 1 },
                    { 43, 56, "Hour 3", 2 },
                    { 44, 56, "Hour 4", 3 },
                    { 45, 56, "Hour 5", 4 },
                    { 46, 56, "Hour 6", 5 },
                    { 47, 56, "Hour 7", 6 },
                    { 48, 56, "Hour 8", 7 },
                    { 49, 57, "Hour 1", 0 },
                    { 50, 57, "Hour 2", 1 },
                    { 51, 57, "Hour 3", 2 },
                    { 52, 57, "Hour 4", 3 },
                    { 53, 57, "Hour 5", 4 },
                    { 54, 57, "Hour 6", 5 },
                    { 55, 57, "Hour 7", 6 },
                    { 56, 57, "Hour 8", 7 },
                    { 57, 58, "Hour 1", 0 },
                    { 58, 58, "Hour 2", 1 },
                    { 59, 58, "Hour 3", 2 },
                    { 60, 58, "Hour 4", 3 },
                    { 61, 58, "Hour 5", 4 },
                    { 62, 58, "Hour 6", 5 },
                    { 63, 58, "Hour 7", 6 },
                    { 64, 58, "Hour 8", 7 },
                    { 65, 59, "Hour 1", 0 },
                    { 66, 59, "Hour 2", 1 },
                    { 67, 59, "Hour 3", 2 },
                    { 68, 59, "Hour 4", 3 },
                    { 69, 59, "Hour 5", 4 },
                    { 70, 59, "Hour 6", 5 },
                    { 71, 59, "Hour 7", 6 },
                    { 72, 59, "Hour 8", 7 },
                    { 73, 60, "Hour 1", 0 },
                    { 74, 60, "Hour 2", 1 },
                    { 75, 60, "Hour 3", 2 },
                    { 76, 60, "Hour 4", 3 },
                    { 77, 60, "Hour 5", 4 },
                    { 78, 60, "Hour 6", 5 },
                    { 79, 60, "Hour 7", 6 },
                    { 80, 60, "Hour 8", 7 },
                    { 81, 77, "Hour 1", 0 },
                    { 82, 77, "Hour 2", 1 },
                    { 83, 77, "Hour 3", 2 },
                    { 84, 77, "Hour 4", 3 },
                    { 85, 77, "Hour 5", 4 },
                    { 86, 77, "Hour 6", 5 },
                    { 87, 77, "Hour 7", 6 },
                    { 88, 77, "Hour 8", 7 },
                    { 89, 78, "Hour 1", 0 },
                    { 90, 78, "Hour 2", 1 },
                    { 91, 78, "Hour 3", 2 },
                    { 92, 78, "Hour 4", 3 },
                    { 93, 78, "Hour 5", 4 },
                    { 94, 78, "Hour 6", 5 },
                    { 95, 78, "Hour 7", 6 },
                    { 96, 78, "Hour 8", 7 },
                    { 97, 79, "Hour 1", 0 },
                    { 98, 79, "Hour 2", 1 },
                    { 99, 79, "Hour 3", 2 },
                    { 100, 79, "Hour 4", 3 },
                    { 101, 79, "Hour 5", 4 },
                    { 102, 79, "Hour 6", 5 },
                    { 103, 79, "Hour 7", 6 },
                    { 104, 79, "Hour 8", 7 },
                    { 105, 80, "Hour 1", 0 },
                    { 106, 80, "Hour 2", 1 },
                    { 107, 80, "Hour 3", 2 },
                    { 108, 80, "Hour 4", 3 },
                    { 109, 80, "Hour 5", 4 },
                    { 110, 80, "Hour 6", 5 },
                    { 111, 80, "Hour 7", 6 },
                    { 112, 80, "Hour 8", 7 },
                    { 113, 81, "Hour 1", 0 },
                    { 114, 81, "Hour 2", 1 },
                    { 115, 81, "Hour 3", 2 },
                    { 116, 81, "Hour 4", 3 },
                    { 117, 81, "Hour 5", 4 },
                    { 118, 81, "Hour 6", 5 },
                    { 119, 81, "Hour 7", 6 },
                    { 120, 81, "Hour 8", 7 },
                    { 121, 82, "Hour 1", 0 },
                    { 122, 82, "Hour 2", 1 },
                    { 123, 82, "Hour 3", 2 },
                    { 124, 82, "Hour 4", 3 },
                    { 125, 82, "Hour 5", 4 },
                    { 126, 82, "Hour 6", 5 },
                    { 127, 82, "Hour 7", 6 },
                    { 128, 82, "Hour 8", 7 },
                    { 129, 83, "Hour 1", 0 },
                    { 130, 83, "Hour 2", 1 },
                    { 131, 83, "Hour 3", 2 },
                    { 132, 83, "Hour 4", 3 },
                    { 133, 83, "Hour 5", 4 },
                    { 134, 83, "Hour 6", 5 },
                    { 135, 83, "Hour 7", 6 },
                    { 136, 83, "Hour 8", 7 },
                    { 137, 84, "Hour 1", 0 },
                    { 138, 84, "Hour 2", 1 },
                    { 139, 84, "Hour 3", 2 },
                    { 140, 84, "Hour 4", 3 },
                    { 141, 84, "Hour 5", 4 },
                    { 142, 84, "Hour 6", 5 },
                    { 143, 84, "Hour 7", 6 },
                    { 144, 84, "Hour 8", 7 },
                    { 145, 101, "Hour 1", 0 },
                    { 146, 101, "Hour 2", 1 },
                    { 147, 101, "Hour 3", 2 },
                    { 148, 101, "Hour 4", 3 },
                    { 149, 101, "Hour 5", 4 },
                    { 150, 101, "Hour 6", 5 },
                    { 151, 101, "Hour 7", 6 },
                    { 152, 101, "Hour 8", 7 },
                    { 153, 102, "Hour 1", 0 },
                    { 154, 102, "Hour 2", 1 },
                    { 155, 102, "Hour 3", 2 },
                    { 156, 102, "Hour 4", 3 },
                    { 157, 102, "Hour 5", 4 },
                    { 158, 102, "Hour 6", 5 },
                    { 159, 102, "Hour 7", 6 },
                    { 160, 102, "Hour 8", 7 },
                    { 161, 103, "Hour 1", 0 },
                    { 162, 103, "Hour 2", 1 },
                    { 163, 103, "Hour 3", 2 },
                    { 164, 103, "Hour 4", 3 },
                    { 165, 103, "Hour 5", 4 },
                    { 166, 103, "Hour 6", 5 },
                    { 167, 103, "Hour 7", 6 },
                    { 168, 103, "Hour 8", 7 },
                    { 169, 104, "Hour 1", 0 },
                    { 170, 104, "Hour 2", 1 },
                    { 171, 104, "Hour 3", 2 },
                    { 172, 104, "Hour 4", 3 },
                    { 173, 104, "Hour 5", 4 },
                    { 174, 104, "Hour 6", 5 },
                    { 175, 104, "Hour 7", 6 },
                    { 176, 104, "Hour 8", 7 },
                    { 177, 105, "Hour 1", 0 },
                    { 178, 105, "Hour 2", 1 },
                    { 179, 105, "Hour 3", 2 },
                    { 180, 105, "Hour 4", 3 },
                    { 181, 105, "Hour 5", 4 },
                    { 182, 105, "Hour 6", 5 },
                    { 183, 105, "Hour 7", 6 },
                    { 184, 105, "Hour 8", 7 },
                    { 185, 106, "Hour 1", 0 },
                    { 186, 106, "Hour 2", 1 },
                    { 187, 106, "Hour 3", 2 },
                    { 188, 106, "Hour 4", 3 },
                    { 189, 106, "Hour 5", 4 },
                    { 190, 106, "Hour 6", 5 },
                    { 191, 106, "Hour 7", 6 },
                    { 192, 106, "Hour 8", 7 },
                    { 193, 107, "Hour 1", 0 },
                    { 194, 107, "Hour 2", 1 },
                    { 195, 107, "Hour 3", 2 },
                    { 196, 107, "Hour 4", 3 },
                    { 197, 107, "Hour 5", 4 },
                    { 198, 107, "Hour 6", 5 },
                    { 199, 107, "Hour 7", 6 },
                    { 200, 107, "Hour 8", 7 },
                    { 201, 108, "Hour 1", 0 },
                    { 202, 108, "Hour 2", 1 },
                    { 203, 108, "Hour 3", 2 },
                    { 204, 108, "Hour 4", 3 },
                    { 205, 108, "Hour 5", 4 },
                    { 206, 108, "Hour 6", 5 },
                    { 207, 108, "Hour 7", 6 },
                    { 208, 108, "Hour 8", 7 },
                    { 209, 125, "Hour 1", 0 },
                    { 210, 125, "Hour 2", 1 },
                    { 211, 125, "Hour 3", 2 },
                    { 212, 125, "Hour 4", 3 },
                    { 213, 125, "Hour 5", 4 },
                    { 214, 125, "Hour 6", 5 },
                    { 215, 125, "Hour 7", 6 },
                    { 216, 125, "Hour 8", 7 },
                    { 217, 126, "Hour 1", 0 },
                    { 218, 126, "Hour 2", 1 },
                    { 219, 126, "Hour 3", 2 },
                    { 220, 126, "Hour 4", 3 },
                    { 221, 126, "Hour 5", 4 },
                    { 222, 126, "Hour 6", 5 },
                    { 223, 126, "Hour 7", 6 },
                    { 224, 126, "Hour 8", 7 },
                    { 225, 127, "Hour 1", 0 },
                    { 226, 127, "Hour 2", 1 },
                    { 227, 127, "Hour 3", 2 },
                    { 228, 127, "Hour 4", 3 },
                    { 229, 127, "Hour 5", 4 },
                    { 230, 127, "Hour 6", 5 },
                    { 231, 127, "Hour 7", 6 },
                    { 232, 127, "Hour 8", 7 },
                    { 233, 128, "Hour 1", 0 },
                    { 234, 128, "Hour 2", 1 },
                    { 235, 128, "Hour 3", 2 },
                    { 236, 128, "Hour 4", 3 },
                    { 237, 128, "Hour 5", 4 },
                    { 238, 128, "Hour 6", 5 },
                    { 239, 128, "Hour 7", 6 },
                    { 240, 128, "Hour 8", 7 },
                    { 241, 129, "Hour 1", 0 },
                    { 242, 129, "Hour 2", 1 },
                    { 243, 129, "Hour 3", 2 },
                    { 244, 129, "Hour 4", 3 },
                    { 245, 129, "Hour 5", 4 },
                    { 246, 129, "Hour 6", 5 },
                    { 247, 129, "Hour 7", 6 },
                    { 248, 129, "Hour 8", 7 },
                    { 249, 130, "Hour 1", 0 },
                    { 250, 130, "Hour 2", 1 },
                    { 251, 130, "Hour 3", 2 },
                    { 252, 130, "Hour 4", 3 },
                    { 253, 130, "Hour 5", 4 },
                    { 254, 130, "Hour 6", 5 },
                    { 255, 130, "Hour 7", 6 },
                    { 256, 130, "Hour 8", 7 },
                    { 257, 131, "Hour 1", 0 },
                    { 258, 131, "Hour 2", 1 },
                    { 259, 131, "Hour 3", 2 },
                    { 260, 131, "Hour 4", 3 },
                    { 261, 131, "Hour 5", 4 },
                    { 262, 131, "Hour 6", 5 },
                    { 263, 131, "Hour 7", 6 },
                    { 264, 131, "Hour 8", 7 },
                    { 265, 132, "Hour 1", 0 },
                    { 266, 132, "Hour 2", 1 },
                    { 267, 132, "Hour 3", 2 },
                    { 268, 132, "Hour 4", 3 },
                    { 269, 132, "Hour 5", 4 },
                    { 270, 132, "Hour 6", 5 },
                    { 271, 132, "Hour 7", 6 },
                    { 272, 132, "Hour 8", 7 },
                });
            migrationBuilder.InsertData(
                table: "RosterPeople",
                columns: new[] { "Id", "ListKind", "Name", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 1, "Hod", "George Thompson", 1, true },
                    { 2, "Hod", "Lukasz Jaworski", 2, true },
                    { 3, "Hod", "Alison Gilley", 3, true },
                    { 4, "Hod", "Piotr Pelka", 4, true },
                    { 5, "Hod", "Michael Tregillis", 5, true },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "RosterPeople", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "RosterPeople", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "RosterPeople", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "RosterPeople", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "RosterPeople", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 272);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 271);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 270);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 269);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 268);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 267);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 266);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 265);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 264);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 263);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 262);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 261);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 260);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 259);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 258);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 257);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 256);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 255);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 254);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 253);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 252);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 251);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 250);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 249);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 248);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 247);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 246);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 245);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 244);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 243);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 242);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 241);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 240);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 239);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 238);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 237);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 236);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 235);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 234);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 233);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 232);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 231);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 230);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 229);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 228);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 227);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 226);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 225);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 224);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 223);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 222);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 221);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 220);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 219);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 218);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 217);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 216);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 215);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 214);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 213);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 212);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 211);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 210);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 209);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 208);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 207);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 206);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 205);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 204);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 203);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 202);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 201);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 200);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 199);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 198);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 197);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 196);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 195);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 194);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 193);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 192);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 191);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 190);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 189);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 188);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 187);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 186);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 185);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 184);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 183);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 182);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 181);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 180);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 179);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 178);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 177);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 176);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 175);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 174);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 173);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 172);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 171);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 170);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 169);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 168);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 167);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 166);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 165);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 164);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 163);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 162);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 161);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 160);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 159);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 158);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 157);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 156);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 155);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 154);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 153);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 152);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 151);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 150);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 149);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 148);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 147);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 146);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 145);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 144);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 143);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 142);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 141);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 140);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 139);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 138);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 137);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 136);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 135);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 134);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 133);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 132);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 131);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 130);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 129);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 128);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 127);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 126);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 125);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 124);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 123);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 122);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 121);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 120);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 119);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 118);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 117);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 116);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 115);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 114);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 113);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 112);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 111);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 110);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 109);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 108);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 107);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 106);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 105);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 104);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 103);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 102);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 101);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 100);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 99);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 98);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 97);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 96);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 95);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 94);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 93);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 92);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 91);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 90);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 89);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 88);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 87);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 86);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 85);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 84);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 83);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 82);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 81);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 80);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 79);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 78);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 77);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 76);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 75);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 74);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 73);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 72);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 71);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 70);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 69);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 68);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 67);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 66);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 65);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 64);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 63);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 62);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 61);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 60);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 59);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 58);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 57);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 56);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 55);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 54);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 53);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 52);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 51);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 50);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 49);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 48);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 47);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 46);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 45);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 44);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 43);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 42);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 41);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 40);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 39);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 38);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 37);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 36);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 35);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 34);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 33);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 32);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 31);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 30);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 29);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 28);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 27);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 26);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 25);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 24);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 23);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 22);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 21);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 20);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 19);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 18);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 17);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 148);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 147);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 146);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 145);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 144);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 143);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 142);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 141);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 140);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 139);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 138);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 137);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 136);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 135);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 134);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 133);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 132);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 131);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 130);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 129);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 128);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 127);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 126);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 125);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 124);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 123);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 122);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 121);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 120);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 119);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 118);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 117);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 116);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 115);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 114);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 113);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 112);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 111);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 110);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 109);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 108);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 107);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 106);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 105);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 104);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 103);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 102);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 101);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 100);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 99);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 98);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 97);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 96);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 95);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 94);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 93);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 92);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 91);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 90);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 89);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 88);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 87);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 86);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 85);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 84);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 83);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 82);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 81);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 80);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 79);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 78);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 77);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 76);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 75);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 74);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 73);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 72);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 71);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 70);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 69);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 68);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 67);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 66);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 65);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 64);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 63);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 62);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 61);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 60);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 59);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 58);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 57);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 56);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 55);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 54);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 53);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 12);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 11);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 10);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 9);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 8);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 7);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 6);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "Areas", keyColumn: "Id", keyValue: 3);
        }
    }
}
