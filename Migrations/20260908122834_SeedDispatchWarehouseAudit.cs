using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAuditSystem.Migrations
{
    /// <summary>
    /// Seeds Dispatch's Warehouse Audit — transcribed from the supplied spreadsheet.
    /// Dispatch had no areas or shifts yet; this creates a single "Warehouse" area and
    /// the site's 1st/2nd/3rd/Continental nights shift pattern (mirroring Stores, since
    /// no shift-specific content was given and it's the same facility), then seeds the
    /// same 24-item audit identically across all four — category, cadence, escalation
    /// window and accountable role are folded into each task's text since the schema
    /// has no dedicated columns for them. Hourly-cadence items are time-boxed with
    /// generic Hour 1-8 checkpoints (no specific clock times were given in the source).
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
                columns: new[] { "Id", "TaskListId", "Text", "SortOrder", "IsTimeBoxed" },
                values: new object[,]
                {
                    { 53, 9, "H&S (Hourly): Walkways, fire exits and emergency routes are clear. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 0, true },
                    { 54, 9, "H&S (Hourly): PPE requirements are being followed. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 1, true },
                    { 55, 9, "H&S (Hourly): No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor). Checked by the Senior Operator; resolve immediately.", 2, true },
                    { 56, 9, "H&S (Hourly): Loading bays, dock levellers and vehicle Security is safe (Rite Height). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 3, true },
                    { 57, 9, "Quality (Hourly): Correct product, quantity and destination are being picked (Bay Sheets). Checked by the Senior Operator; resolve within the same shift.", 4, true },
                    { 58, 9, "Quality (Hourly): Labels, paperwork and scanning are accurate (Check Scanner V Loading). Checked by the Senior Operator; resolve within the same shift.", 5, true },
                    { 59, 9, "Performance (Hourly): Picking and despatch activity is on plan. Checked by the Senior Operator; resolve within the same shift.", 6, true },
                    { 60, 9, "Performance (Hourly): Bottlenecks, downtime and waiting vehicles are controlled. Checked by the Senior Operator; resolve within the same shift.", 7, true },
                    { 61, 9, "H&S (Per Shift): Pre-use equipment checks are completed and defects reported (Check Booklets). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 8, false },
                    { 62, 9, "H&S (Per Shift): Spill kits, first-aid and fire points are accessible. Checked by the Senior Operator; resolve immediately.", 9, false },
                    { 63, 9, "H&S (Per Shift): Pedestrian and MHE segregation controls are effective. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 10, false },
                    { 64, 9, "Quality (Per Shift): Packaging standards and load security are acceptable. Checked by the Senior Operator; resolve within the same shift.", 11, false },
                    { 65, 9, "Quality (Per Shift): Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card. Checked by the Senior Operator; resolve within the same shift.", 12, false },
                    { 66, 9, "Quality (Per Shift): FIFO/stock rotation requirements are followed where applicable (New). Checked by the Senior Operator; resolve within the same shift.", 13, false },
                    { 67, 9, "Performance (Per Shift): Shift plan, priorities and cut-off times were communicated (Shift Start Up). Checked by the HOD; resolve within the same shift.", 14, false },
                    { 68, 9, "Performance (Per Shift): Labour and equipment resources are adequate for the plan. Checked by the Senior Operator; resolve within the same shift.", 15, false },
                    { 69, 9, "Performance (Per Shift): Shift KPIs, backlog and carry-over work were reviewed (RPS). Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 16, false },
                    { 70, 9, "Morale (Per Shift): Team brief and safety message were completed ( Start of Each Shift Team Brief). Checked by the HOD; resolve within the same shift.", 17, false },
                    { 71, 9, "Morale (Per Shift): Team concerns, support needs and training gaps were discussed. Checked by the HOD; resolve by the next shift.", 18, false },
                    { 72, 9, "Morale (Per Shift): Good performance and positive behaviours were recognised. Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 19, false },
                    { 73, 9, "H&S (Daily): General housekeeping and waste controls meet standard. Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 20, false },
                    { 74, 9, "Quality (Daily): A sample of completed orders was checked for accuracy ( PickList V Shipping Documents). Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 21, false },
                    { 75, 9, "Performance (Daily): Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows). Checked by the Senior Operator; resolve within the same day.", 22, false },
                    { 76, 9, "Morale (Daily): Absence, overtime, workload and welfare concerns were reviewed. Checked by the HOD; resolve within the same day.", 23, false },
                    { 77, 10, "H&S (Hourly): Walkways, fire exits and emergency routes are clear. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 0, true },
                    { 78, 10, "H&S (Hourly): PPE requirements are being followed. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 1, true },
                    { 79, 10, "H&S (Hourly): No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor). Checked by the Senior Operator; resolve immediately.", 2, true },
                    { 80, 10, "H&S (Hourly): Loading bays, dock levellers and vehicle Security is safe (Rite Height). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 3, true },
                    { 81, 10, "Quality (Hourly): Correct product, quantity and destination are being picked (Bay Sheets). Checked by the Senior Operator; resolve within the same shift.", 4, true },
                    { 82, 10, "Quality (Hourly): Labels, paperwork and scanning are accurate (Check Scanner V Loading). Checked by the Senior Operator; resolve within the same shift.", 5, true },
                    { 83, 10, "Performance (Hourly): Picking and despatch activity is on plan. Checked by the Senior Operator; resolve within the same shift.", 6, true },
                    { 84, 10, "Performance (Hourly): Bottlenecks, downtime and waiting vehicles are controlled. Checked by the Senior Operator; resolve within the same shift.", 7, true },
                    { 85, 10, "H&S (Per Shift): Pre-use equipment checks are completed and defects reported (Check Booklets). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 8, false },
                    { 86, 10, "H&S (Per Shift): Spill kits, first-aid and fire points are accessible. Checked by the Senior Operator; resolve immediately.", 9, false },
                    { 87, 10, "H&S (Per Shift): Pedestrian and MHE segregation controls are effective. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 10, false },
                    { 88, 10, "Quality (Per Shift): Packaging standards and load security are acceptable. Checked by the Senior Operator; resolve within the same shift.", 11, false },
                    { 89, 10, "Quality (Per Shift): Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card. Checked by the Senior Operator; resolve within the same shift.", 12, false },
                    { 90, 10, "Quality (Per Shift): FIFO/stock rotation requirements are followed where applicable (New). Checked by the Senior Operator; resolve within the same shift.", 13, false },
                    { 91, 10, "Performance (Per Shift): Shift plan, priorities and cut-off times were communicated (Shift Start Up). Checked by the HOD; resolve within the same shift.", 14, false },
                    { 92, 10, "Performance (Per Shift): Labour and equipment resources are adequate for the plan. Checked by the Senior Operator; resolve within the same shift.", 15, false },
                    { 93, 10, "Performance (Per Shift): Shift KPIs, backlog and carry-over work were reviewed (RPS). Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 16, false },
                    { 94, 10, "Morale (Per Shift): Team brief and safety message were completed ( Start of Each Shift Team Brief). Checked by the HOD; resolve within the same shift.", 17, false },
                    { 95, 10, "Morale (Per Shift): Team concerns, support needs and training gaps were discussed. Checked by the HOD; resolve by the next shift.", 18, false },
                    { 96, 10, "Morale (Per Shift): Good performance and positive behaviours were recognised. Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 19, false },
                    { 97, 10, "H&S (Daily): General housekeeping and waste controls meet standard. Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 20, false },
                    { 98, 10, "Quality (Daily): A sample of completed orders was checked for accuracy ( PickList V Shipping Documents). Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 21, false },
                    { 99, 10, "Performance (Daily): Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows). Checked by the Senior Operator; resolve within the same day.", 22, false },
                    { 100, 10, "Morale (Daily): Absence, overtime, workload and welfare concerns were reviewed. Checked by the HOD; resolve within the same day.", 23, false },
                    { 101, 11, "H&S (Hourly): Walkways, fire exits and emergency routes are clear. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 0, true },
                    { 102, 11, "H&S (Hourly): PPE requirements are being followed. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 1, true },
                    { 103, 11, "H&S (Hourly): No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor). Checked by the Senior Operator; resolve immediately.", 2, true },
                    { 104, 11, "H&S (Hourly): Loading bays, dock levellers and vehicle Security is safe (Rite Height). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 3, true },
                    { 105, 11, "Quality (Hourly): Correct product, quantity and destination are being picked (Bay Sheets). Checked by the Senior Operator; resolve within the same shift.", 4, true },
                    { 106, 11, "Quality (Hourly): Labels, paperwork and scanning are accurate (Check Scanner V Loading). Checked by the Senior Operator; resolve within the same shift.", 5, true },
                    { 107, 11, "Performance (Hourly): Picking and despatch activity is on plan. Checked by the Senior Operator; resolve within the same shift.", 6, true },
                    { 108, 11, "Performance (Hourly): Bottlenecks, downtime and waiting vehicles are controlled. Checked by the Senior Operator; resolve within the same shift.", 7, true },
                    { 109, 11, "H&S (Per Shift): Pre-use equipment checks are completed and defects reported (Check Booklets). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 8, false },
                    { 110, 11, "H&S (Per Shift): Spill kits, first-aid and fire points are accessible. Checked by the Senior Operator; resolve immediately.", 9, false },
                    { 111, 11, "H&S (Per Shift): Pedestrian and MHE segregation controls are effective. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 10, false },
                    { 112, 11, "Quality (Per Shift): Packaging standards and load security are acceptable. Checked by the Senior Operator; resolve within the same shift.", 11, false },
                    { 113, 11, "Quality (Per Shift): Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card. Checked by the Senior Operator; resolve within the same shift.", 12, false },
                    { 114, 11, "Quality (Per Shift): FIFO/stock rotation requirements are followed where applicable (New). Checked by the Senior Operator; resolve within the same shift.", 13, false },
                    { 115, 11, "Performance (Per Shift): Shift plan, priorities and cut-off times were communicated (Shift Start Up). Checked by the HOD; resolve within the same shift.", 14, false },
                    { 116, 11, "Performance (Per Shift): Labour and equipment resources are adequate for the plan. Checked by the Senior Operator; resolve within the same shift.", 15, false },
                    { 117, 11, "Performance (Per Shift): Shift KPIs, backlog and carry-over work were reviewed (RPS). Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 16, false },
                    { 118, 11, "Morale (Per Shift): Team brief and safety message were completed ( Start of Each Shift Team Brief). Checked by the HOD; resolve within the same shift.", 17, false },
                    { 119, 11, "Morale (Per Shift): Team concerns, support needs and training gaps were discussed. Checked by the HOD; resolve by the next shift.", 18, false },
                    { 120, 11, "Morale (Per Shift): Good performance and positive behaviours were recognised. Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 19, false },
                    { 121, 11, "H&S (Daily): General housekeeping and waste controls meet standard. Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 20, false },
                    { 122, 11, "Quality (Daily): A sample of completed orders was checked for accuracy ( PickList V Shipping Documents). Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 21, false },
                    { 123, 11, "Performance (Daily): Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows). Checked by the Senior Operator; resolve within the same day.", 22, false },
                    { 124, 11, "Morale (Daily): Absence, overtime, workload and welfare concerns were reviewed. Checked by the HOD; resolve within the same day.", 23, false },
                    { 125, 12, "H&S (Hourly): Walkways, fire exits and emergency routes are clear. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 0, true },
                    { 126, 12, "H&S (Hourly): PPE requirements are being followed. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 1, true },
                    { 127, 12, "H&S (Hourly): No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor). Checked by the Senior Operator; resolve immediately.", 2, true },
                    { 128, 12, "H&S (Hourly): Loading bays, dock levellers and vehicle Security is safe (Rite Height). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 3, true },
                    { 129, 12, "Quality (Hourly): Correct product, quantity and destination are being picked (Bay Sheets). Checked by the Senior Operator; resolve within the same shift.", 4, true },
                    { 130, 12, "Quality (Hourly): Labels, paperwork and scanning are accurate (Check Scanner V Loading). Checked by the Senior Operator; resolve within the same shift.", 5, true },
                    { 131, 12, "Performance (Hourly): Picking and despatch activity is on plan. Checked by the Senior Operator; resolve within the same shift.", 6, true },
                    { 132, 12, "Performance (Hourly): Bottlenecks, downtime and waiting vehicles are controlled. Checked by the Senior Operator; resolve within the same shift.", 7, true },
                    { 133, 12, "H&S (Per Shift): Pre-use equipment checks are completed and defects reported (Check Booklets). Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 8, false },
                    { 134, 12, "H&S (Per Shift): Spill kits, first-aid and fire points are accessible. Checked by the Senior Operator; resolve immediately.", 9, false },
                    { 135, 12, "H&S (Per Shift): Pedestrian and MHE segregation controls are effective. Checked by the Senior Operator; escalate to HOD immediately if not resolved.", 10, false },
                    { 136, 12, "Quality (Per Shift): Packaging standards and load security are acceptable. Checked by the Senior Operator; resolve within the same shift.", 11, false },
                    { 137, 12, "Quality (Per Shift): Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card. Checked by the Senior Operator; resolve within the same shift.", 12, false },
                    { 138, 12, "Quality (Per Shift): FIFO/stock rotation requirements are followed where applicable (New). Checked by the Senior Operator; resolve within the same shift.", 13, false },
                    { 139, 12, "Performance (Per Shift): Shift plan, priorities and cut-off times were communicated (Shift Start Up). Checked by the HOD; resolve within the same shift.", 14, false },
                    { 140, 12, "Performance (Per Shift): Labour and equipment resources are adequate for the plan. Checked by the Senior Operator; resolve within the same shift.", 15, false },
                    { 141, 12, "Performance (Per Shift): Shift KPIs, backlog and carry-over work were reviewed (RPS). Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 16, false },
                    { 142, 12, "Morale (Per Shift): Team brief and safety message were completed ( Start of Each Shift Team Brief). Checked by the HOD; resolve within the same shift.", 17, false },
                    { 143, 12, "Morale (Per Shift): Team concerns, support needs and training gaps were discussed. Checked by the HOD; resolve by the next shift.", 18, false },
                    { 144, 12, "Morale (Per Shift): Good performance and positive behaviours were recognised. Checked by the Senior Operator; escalate to HOD by the next shift if not resolved.", 19, false },
                    { 145, 12, "H&S (Daily): General housekeeping and waste controls meet standard. Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 20, false },
                    { 146, 12, "Quality (Daily): A sample of completed orders was checked for accuracy ( PickList V Shipping Documents). Checked by the Senior Operator; escalate to HOD within the same day if not resolved.", 21, false },
                    { 147, 12, "Performance (Daily): Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows). Checked by the Senior Operator; resolve within the same day.", 22, false },
                    { 148, 12, "Morale (Daily): Absence, overtime, workload and welfare concerns were reviewed. Checked by the HOD; resolve within the same day.", 23, false },
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
