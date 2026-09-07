using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAuditSystem.Migrations
{
    /// <summary>
    /// Seeds the Stores and Dispatch departments, the Stores areas, shifts and the
    /// four task lists transcribed verbatim from the paper Safe Start forms (SHEF014).
    /// Everything here is data — the same rows an admin could create by hand — so no
    /// department, area, shift or task text lives in code.
    /// </summary>
    public partial class SeedSupportData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 1, "Stores", 1, true },
                    { 2, "Dispatch", 2, true },
                });
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "DepartmentId", "Name", "DefaultLocation", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, "Stores DP1 & DP3", "DP1 & DP3", 1, true },
                    { 2, 1, "Consumables", "Consumables", 2, true },
                });
            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "DepartmentId", "Name", "SortOrder", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, "1st", 1, true },
                    { 2, 1, "2nd", 2, true },
                    { 3, 1, "3rd", 3, true },
                    { 4, 1, "Continental nights", 4, true },
                });
            migrationBuilder.InsertData(
                table: "TaskLists",
                columns: new[] { "Id", "AreaId", "ShiftId", "Version", "IsCurrent", "HealthRepsReminder", "CreatedAt", "CreatedBy" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 2, 1, 2, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 3, 1, 3, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 4, 1, 4, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 5, 2, 1, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 6, 2, 2, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 7, 2, 3, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                    { 8, 2, 4, 1, true, "Please remember to check on the health and wellbeing of your team throughout the shift, and make sure you know who your Health & Safety Representatives are.", new System.DateTime(2026, 1, 1, 0, 0, 0, System.DateTimeKind.Utc), "System seed" },
                });
            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "TaskListId", "Text", "SortOrder", "IsTimeBoxed" },
                values: new object[,]
                {
                    { 1, 1, "Complete pre shift check book been done on the forklift/ ppt and pallet trucks", 0, false },
                    { 2, 1, "Tidy the yard at both ends and ensure returns ready for collection: pallets / stillages & German returns", 1, false },
                    { 3, 1, "order everything for today’s production (spc / AMC TRACKING sheet)", 2, false },
                    { 4, 1, "stock check the gas at dp1 and raise any shortages. correct if needed on sap", 3, false },
                    { 5, 1, "Check that the gas cage is tidy and all bottles stored safely, action if not", 4, false },
                    { 6, 1, "Count pallet trailers and raise potential shortages for the next shift - Escalate any issues.", 5, false },
                    { 7, 1, "send any parcels to consumables to be processed – first batch by 9.30am", 6, false },
                    { 8, 1, "send any parcels to consumables to be processed – Batch 2 by 11.30am", 7, false },
                    { 9, 1, "send any parcels to consumables to be processed – batch 3 by 1.30pm", 8, false },
                    { 10, 1, "Inspect racking areas and report any damage, also remove any unnecessary banding or shrink wrap you find hanging from the racks", 9, false },
                    { 11, 1, "inspect floors and fire exit and ensure they are clear of obstructions. IE: Pallets, stillages and empty crates", 10, true },
                    { 12, 1, "Check verticals on order to replace any pallets used", 11, true },
                    { 13, 1, "Inspect DP1 – move any pallets of steel over hanging the WALKWAYS and ensure coils are not exceeding the blue height restriction line", 12, false },
                    { 14, 1, "inspect pallets in stores and ensure we have no sharp edge pallets", 13, false },
                    { 15, 1, "sweep through the store and ensure it is tidy for the next shift.", 14, false },
                    { 16, 2, "has the pre shift check books been done on the forklift / ppt and the pallets trucks?", 0, false },
                    { 17, 2, "Have the pallet trailers been counted and any potential shortages been raised for the next shift? Escalate any issues.", 1, false },
                    { 18, 2, "Have we ordered the tx so it’s on site for 23:00 start", 2, false },
                    { 19, 2, "Have we located everything that was booked in by the previous shift and cleared option 1 put away?", 3, false },
                    { 20, 2, "is the mla area clean and tidy with no returned stock just sitting around. If SO, please return to the correct stores", 4, false },
                    { 21, 2, "Check the racking for anything hanging down and correct this IE: banding, shrink wrap etc.", 5, false },
                    { 22, 2, "Has the parcel shelf been sent to consumables to be processed? Every 2 hours", 6, true },
                    { 23, 2, "Are racking areas safe? And all damage if any has been reported", 7, false },
                    { 24, 2, "Are floors and fire exit clear of obstructions. IE: Pallets, stillages and empty crates", 8, false },
                    { 25, 2, "have the scrap caps been loaded onto the Ferguson’s trailer", 9, false },
                    { 26, 2, "Have we cycle counted the pallet trailers and corrected the stocks", 10, false },
                    { 27, 2, "have the verticals been ordered every time we issue a pallet so we have constant stock flow?", 11, false },
                    { 28, 2, "Are pallets of steel over hanging the WALKWAYS in dp1 and not exceeding the blue hight restriction line?", 12, false },
                    { 29, 2, "are the operators ENSURING that we have no sharp edge pallets in the stores", 13, false },
                    { 30, 2, "have we swept through the store and is it tidy for the next shift.", 14, false },
                    { 31, 3, "Has the pre shift check book been done on the forklift/ ppt and pallet trucks?", 0, false },
                    { 32, 3, "Are racking areas safe? And all damage if any has been reported", 1, false },
                    { 33, 3, "Are all pallets strapped and stored safely at DP1 / DP3", 2, false },
                    { 34, 3, "Is the TX on the line and not still in stores?", 3, false },
                    { 35, 3, "Have the parcels been sent to consumables to be processed? Every two hours on this!", 4, true },
                    { 36, 3, "Are floors and fire exit clear of obstructions. IE: Pallets, stillages and empty crates", 5, false },
                    { 37, 3, "Do the racks have any banding or shrink wrap hanging from them that needs removing?", 6, false },
                    { 38, 3, "Are pallets of steel over hanging the WALKWAYS in dp1 and not exceeding the blue hight restriction line?", 7, false },
                    { 39, 3, "Are the cycle counts done for the day and if not please ensure they are finished.", 8, false },
                    { 40, 3, "are the operators ENSURING that we have no sharp edge pallets in the stores", 9, false },
                    { 41, 3, "Is the put away clear for the next shift and all deliveries put away?", 10, false },
                    { 42, 3, "have we swept through the store and is it tidy for the next shift.", 11, false },
                    { 43, 4, "Has the pre shift check book been done on the forklift/ ppt and pallet trucks?", 0, false },
                    { 44, 4, "Are racking areas safe? And all damage if any has been reported", 1, false },
                    { 45, 4, "Are all pallets strapped and stored safely at DP1", 2, false },
                    { 46, 4, "Is the coils area safe and nothing blocking the fire exit or obstructing the WALKWAYS", 3, false },
                    { 47, 4, "Do the racks have any banding or shrink wrap hanging from them that needs removing?", 4, false },
                    { 48, 4, "Are pallets of steel over hanging the WALKWAYS in dp1 and not exceeding the blue hight restriction line?", 5, false },
                    { 49, 4, "Are the cycle counts done for the day and if not please ensure they are finished.", 6, false },
                    { 50, 4, "Is the put away clear for the next shift and all deliveries put away?", 7, false },
                    { 51, 4, "Have the scrap tables and bins been emptied for the next shift and not left so production is stopped at 07:00", 8, false },
                    { 52, 4, "have we swept through the store and is it tidy for the next shift. If no ask DP3 to support", 9, false },
                });
            migrationBuilder.InsertData(
                table: "TaskCheckpoints",
                columns: new[] { "Id", "TaskItemId", "Label", "SortOrder" },
                values: new object[,]
                {
                    { 1, 11, "9am", 0 },
                    { 2, 11, "11am", 1 },
                    { 3, 11, "1pm", 2 },
                    { 4, 11, "3pm", 3 },
                    { 5, 12, "9am", 0 },
                    { 6, 12, "11am", 1 },
                    { 7, 12, "1pm", 2 },
                    { 8, 12, "3pm", 3 },
                    { 9, 22, "Batch 1", 0 },
                    { 10, 22, "Batch 2", 1 },
                    { 11, 22, "Batch 3", 2 },
                    { 12, 22, "Batch 4", 3 },
                    { 13, 35, "Batch 1", 0 },
                    { 14, 35, "Batch 2", 1 },
                    { 15, 35, "Batch 3", 2 },
                    { 16, 35, "Batch 4", 3 },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 16);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 15);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 14);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 13);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 12);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 11);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 10);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 9);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 8);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 7);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 6);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "TaskCheckpoints", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 52);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 51);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 50);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 49);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 48);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 47);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 46);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 45);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 44);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 43);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 42);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 41);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 40);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 39);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 38);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 37);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 36);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 35);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 34);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 33);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 32);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 31);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 30);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 29);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 28);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 27);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 26);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 25);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 24);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 23);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 22);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 21);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 20);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 19);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 18);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 17);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 16);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 15);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 14);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 13);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 12);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 11);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 10);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 9);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 8);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 7);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 6);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "TaskItems", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 8);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 7);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 6);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "TaskLists", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Shifts", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Areas", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Areas", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Departments", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Departments", keyColumn: "Id", keyValue: 1);
        }
    }
}
