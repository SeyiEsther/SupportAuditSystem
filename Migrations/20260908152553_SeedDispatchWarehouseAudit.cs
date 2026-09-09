using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAuditSystem.Migrations
{
    /// <summary>
    /// Seeds Dispatch's Warehouse Audit — transcribed from the supplied spreadsheet:
    /// a single "Warehouse" area, the site's four shifts, and the same 24-item audit
    /// across all four. Category, cadence, the accountable role and the escalation
    /// target/window are their own columns (not folded into the task text), and the
    /// HOD roster is seeded with the same names as the Production Audit System's.
    ///
    /// Written as idempotent MERGEs rather than plain inserts: an earlier version of
    /// this seed already ran against the live database, so these rows may already
    /// exist. Insert when missing, update in place when present — safe on a fresh
    /// database and on one that already carries the earlier seed, and safe to re-run.
    /// </summary>
    public partial class SeedDispatchWarehouseAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [Areas] ON;
MERGE [Areas] AS t
USING (VALUES
    (3, 2, N'Warehouse', N'Warehouse', 1, 1)
) AS s([Id], [DepartmentId], [Name], [DefaultLocation], [SortOrder], [IsActive])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[DepartmentId] = s.[DepartmentId], t.[Name] = s.[Name], t.[DefaultLocation] = s.[DefaultLocation], t.[SortOrder] = s.[SortOrder], t.[IsActive] = s.[IsActive]
WHEN NOT MATCHED THEN INSERT ([Id], [DepartmentId], [Name], [DefaultLocation], [SortOrder], [IsActive])
    VALUES (s.[Id], s.[DepartmentId], s.[Name], s.[DefaultLocation], s.[SortOrder], s.[IsActive]);
SET IDENTITY_INSERT [Areas] OFF;
");
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [Shifts] ON;
MERGE [Shifts] AS t
USING (VALUES
    (5, 2, N'1st', 1, 1),
    (6, 2, N'2nd', 2, 1),
    (7, 2, N'3rd', 3, 1),
    (8, 2, N'Continental nights', 4, 1)
) AS s([Id], [DepartmentId], [Name], [SortOrder], [IsActive])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[DepartmentId] = s.[DepartmentId], t.[Name] = s.[Name], t.[SortOrder] = s.[SortOrder], t.[IsActive] = s.[IsActive]
WHEN NOT MATCHED THEN INSERT ([Id], [DepartmentId], [Name], [SortOrder], [IsActive])
    VALUES (s.[Id], s.[DepartmentId], s.[Name], s.[SortOrder], s.[IsActive]);
SET IDENTITY_INSERT [Shifts] OFF;
");
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [TaskLists] ON;
MERGE [TaskLists] AS t
USING (VALUES
    (9, 3, 5, 1, 1, NULL, '2026-01-01T00:00:00.000', N'System seed'),
    (10, 3, 6, 1, 1, NULL, '2026-01-01T00:00:00.000', N'System seed'),
    (11, 3, 7, 1, 1, NULL, '2026-01-01T00:00:00.000', N'System seed'),
    (12, 3, 8, 1, 1, NULL, '2026-01-01T00:00:00.000', N'System seed')
) AS s([Id], [AreaId], [ShiftId], [Version], [IsCurrent], [HealthRepsReminder], [CreatedAt], [CreatedBy])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[AreaId] = s.[AreaId], t.[ShiftId] = s.[ShiftId], t.[Version] = s.[Version], t.[IsCurrent] = s.[IsCurrent], t.[HealthRepsReminder] = s.[HealthRepsReminder], t.[CreatedAt] = s.[CreatedAt], t.[CreatedBy] = s.[CreatedBy]
WHEN NOT MATCHED THEN INSERT ([Id], [AreaId], [ShiftId], [Version], [IsCurrent], [HealthRepsReminder], [CreatedAt], [CreatedBy])
    VALUES (s.[Id], s.[AreaId], s.[ShiftId], s.[Version], s.[IsCurrent], s.[HealthRepsReminder], s.[CreatedAt], s.[CreatedBy]);
SET IDENTITY_INSERT [TaskLists] OFF;
");
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [TaskItems] ON;
MERGE [TaskItems] AS t
USING (VALUES
    (53, 9, N'Walkways, fire exits and emergency routes are clear', 0, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (54, 9, N'PPE requirements are being followed', 1, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (55, 9, N'No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)', 2, 1, N'H&S', N'Hourly', N'Senior Operator', NULL, N'immediately'),
    (56, 9, N'Loading bays, dock levellers and vehicle Security is safe (Rite Height)', 3, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (57, 9, N'Correct product, quantity and destination are being picked (Bay Sheets)', 4, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (58, 9, N'Labels, paperwork and scanning are accurate (Check Scanner V Loading)', 5, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (59, 9, N'Picking and despatch activity is on plan', 6, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (60, 9, N'Bottlenecks, downtime and waiting vehicles are controlled', 7, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (61, 9, N'Pre-use equipment checks are completed and defects reported (Check Booklets)', 8, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (62, 9, N'Spill kits, first-aid and fire points are accessible', 9, 0, N'H&S', N'Per Shift', N'Senior Operator', NULL, N'immediately'),
    (63, 9, N'Pedestrian and MHE segregation controls are effective', 10, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (64, 9, N'Packaging standards and load security are acceptable', 11, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (65, 9, N'Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card', 12, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (66, 9, N'FIFO/stock rotation requirements are followed where applicable (New)', 13, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (67, 9, N'Shift plan, priorities and cut-off times were communicated (Shift Start Up)', 14, 0, N'Performance', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (68, 9, N'Labour and equipment resources are adequate for the plan', 15, 0, N'Performance', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (69, 9, N'Shift KPIs, backlog and carry-over work were reviewed (RPS)', 16, 0, N'Performance', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (70, 9, N'Team brief and safety message were completed ( Start of Each Shift Team Brief)', 17, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (71, 9, N'Team concerns, support needs and training gaps were discussed', 18, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'by the next shift'),
    (72, 9, N'Good performance and positive behaviours were recognised', 19, 0, N'Morale', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (73, 9, N'General housekeeping and waste controls meet standard', 20, 0, N'H&S', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (74, 9, N'A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)', 21, 0, N'Quality', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (75, 9, N'Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)', 22, 0, N'Performance', N'Daily', N'Senior Operator', NULL, N'within the same day'),
    (76, 9, N'Absence, overtime, workload and welfare concerns were reviewed', 23, 0, N'Morale', N'Daily', N'HOD', NULL, N'within the same day'),
    (77, 10, N'Walkways, fire exits and emergency routes are clear', 0, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (78, 10, N'PPE requirements are being followed', 1, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (79, 10, N'No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)', 2, 1, N'H&S', N'Hourly', N'Senior Operator', NULL, N'immediately'),
    (80, 10, N'Loading bays, dock levellers and vehicle Security is safe (Rite Height)', 3, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (81, 10, N'Correct product, quantity and destination are being picked (Bay Sheets)', 4, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (82, 10, N'Labels, paperwork and scanning are accurate (Check Scanner V Loading)', 5, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (83, 10, N'Picking and despatch activity is on plan', 6, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (84, 10, N'Bottlenecks, downtime and waiting vehicles are controlled', 7, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (85, 10, N'Pre-use equipment checks are completed and defects reported (Check Booklets)', 8, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (86, 10, N'Spill kits, first-aid and fire points are accessible', 9, 0, N'H&S', N'Per Shift', N'Senior Operator', NULL, N'immediately'),
    (87, 10, N'Pedestrian and MHE segregation controls are effective', 10, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (88, 10, N'Packaging standards and load security are acceptable', 11, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (89, 10, N'Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card', 12, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (90, 10, N'FIFO/stock rotation requirements are followed where applicable (New)', 13, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (91, 10, N'Shift plan, priorities and cut-off times were communicated (Shift Start Up)', 14, 0, N'Performance', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (92, 10, N'Labour and equipment resources are adequate for the plan', 15, 0, N'Performance', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (93, 10, N'Shift KPIs, backlog and carry-over work were reviewed (RPS)', 16, 0, N'Performance', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (94, 10, N'Team brief and safety message were completed ( Start of Each Shift Team Brief)', 17, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (95, 10, N'Team concerns, support needs and training gaps were discussed', 18, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'by the next shift'),
    (96, 10, N'Good performance and positive behaviours were recognised', 19, 0, N'Morale', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (97, 10, N'General housekeeping and waste controls meet standard', 20, 0, N'H&S', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (98, 10, N'A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)', 21, 0, N'Quality', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (99, 10, N'Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)', 22, 0, N'Performance', N'Daily', N'Senior Operator', NULL, N'within the same day'),
    (100, 10, N'Absence, overtime, workload and welfare concerns were reviewed', 23, 0, N'Morale', N'Daily', N'HOD', NULL, N'within the same day'),
    (101, 11, N'Walkways, fire exits and emergency routes are clear', 0, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (102, 11, N'PPE requirements are being followed', 1, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (103, 11, N'No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)', 2, 1, N'H&S', N'Hourly', N'Senior Operator', NULL, N'immediately'),
    (104, 11, N'Loading bays, dock levellers and vehicle Security is safe (Rite Height)', 3, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (105, 11, N'Correct product, quantity and destination are being picked (Bay Sheets)', 4, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (106, 11, N'Labels, paperwork and scanning are accurate (Check Scanner V Loading)', 5, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (107, 11, N'Picking and despatch activity is on plan', 6, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (108, 11, N'Bottlenecks, downtime and waiting vehicles are controlled', 7, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (109, 11, N'Pre-use equipment checks are completed and defects reported (Check Booklets)', 8, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (110, 11, N'Spill kits, first-aid and fire points are accessible', 9, 0, N'H&S', N'Per Shift', N'Senior Operator', NULL, N'immediately'),
    (111, 11, N'Pedestrian and MHE segregation controls are effective', 10, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (112, 11, N'Packaging standards and load security are acceptable', 11, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (113, 11, N'Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card', 12, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (114, 11, N'FIFO/stock rotation requirements are followed where applicable (New)', 13, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (115, 11, N'Shift plan, priorities and cut-off times were communicated (Shift Start Up)', 14, 0, N'Performance', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (116, 11, N'Labour and equipment resources are adequate for the plan', 15, 0, N'Performance', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (117, 11, N'Shift KPIs, backlog and carry-over work were reviewed (RPS)', 16, 0, N'Performance', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (118, 11, N'Team brief and safety message were completed ( Start of Each Shift Team Brief)', 17, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (119, 11, N'Team concerns, support needs and training gaps were discussed', 18, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'by the next shift'),
    (120, 11, N'Good performance and positive behaviours were recognised', 19, 0, N'Morale', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (121, 11, N'General housekeeping and waste controls meet standard', 20, 0, N'H&S', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (122, 11, N'A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)', 21, 0, N'Quality', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (123, 11, N'Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)', 22, 0, N'Performance', N'Daily', N'Senior Operator', NULL, N'within the same day'),
    (124, 11, N'Absence, overtime, workload and welfare concerns were reviewed', 23, 0, N'Morale', N'Daily', N'HOD', NULL, N'within the same day'),
    (125, 12, N'Walkways, fire exits and emergency routes are clear', 0, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (126, 12, N'PPE requirements are being followed', 1, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (127, 12, N'No unsafe stacking, damaged pallets or falling-object risks (Mezz Floor)', 2, 1, N'H&S', N'Hourly', N'Senior Operator', NULL, N'immediately'),
    (128, 12, N'Loading bays, dock levellers and vehicle Security is safe (Rite Height)', 3, 1, N'H&S', N'Hourly', N'Senior Operator', N'HOD', N'immediately'),
    (129, 12, N'Correct product, quantity and destination are being picked (Bay Sheets)', 4, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (130, 12, N'Labels, paperwork and scanning are accurate (Check Scanner V Loading)', 5, 1, N'Quality', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (131, 12, N'Picking and despatch activity is on plan', 6, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (132, 12, N'Bottlenecks, downtime and waiting vehicles are controlled', 7, 1, N'Performance', N'Hourly', N'Senior Operator', NULL, N'within the same shift'),
    (133, 12, N'Pre-use equipment checks are completed and defects reported (Check Booklets)', 8, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (134, 12, N'Spill kits, first-aid and fire points are accessible', 9, 0, N'H&S', N'Per Shift', N'Senior Operator', NULL, N'immediately'),
    (135, 12, N'Pedestrian and MHE segregation controls are effective', 10, 0, N'H&S', N'Per Shift', N'Senior Operator', N'HOD', N'immediately'),
    (136, 12, N'Packaging standards and load security are acceptable', 11, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (137, 12, N'Damaged, quarantined or non-conforming stock is controlled On Hold Products Red Card', 12, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (138, 12, N'FIFO/stock rotation requirements are followed where applicable (New)', 13, 0, N'Quality', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (139, 12, N'Shift plan, priorities and cut-off times were communicated (Shift Start Up)', 14, 0, N'Performance', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (140, 12, N'Labour and equipment resources are adequate for the plan', 15, 0, N'Performance', N'Per Shift', N'Senior Operator', NULL, N'within the same shift'),
    (141, 12, N'Shift KPIs, backlog and carry-over work were reviewed (RPS)', 16, 0, N'Performance', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (142, 12, N'Team brief and safety message were completed ( Start of Each Shift Team Brief)', 17, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'within the same shift'),
    (143, 12, N'Team concerns, support needs and training gaps were discussed', 18, 0, N'Morale', N'Per Shift', N'HOD', NULL, N'by the next shift'),
    (144, 12, N'Good performance and positive behaviours were recognised', 19, 0, N'Morale', N'Per Shift', N'Senior Operator', N'HOD', N'by the next shift'),
    (145, 12, N'General housekeeping and waste controls meet standard', 20, 0, N'H&S', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (146, 12, N'A sample of completed orders was checked for accuracy ( PickList V Shipping Documents)', 21, 0, N'Quality', N'Daily', N'Senior Operator', N'HOD', N'within the same day'),
    (147, 12, N'Daily output, service, productivity and missed deadlines were reviewed ( Transport Late Runners, No Shows)', 22, 0, N'Performance', N'Daily', N'Senior Operator', NULL, N'within the same day'),
    (148, 12, N'Absence, overtime, workload and welfare concerns were reviewed', 23, 0, N'Morale', N'Daily', N'HOD', NULL, N'within the same day')
) AS s([Id], [TaskListId], [Text], [SortOrder], [IsTimeBoxed], [Category], [Cadence], [ResponsibleRole], [EscalateToRole], [EscalationWindow])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[TaskListId] = s.[TaskListId], t.[Text] = s.[Text], t.[SortOrder] = s.[SortOrder], t.[IsTimeBoxed] = s.[IsTimeBoxed], t.[Category] = s.[Category], t.[Cadence] = s.[Cadence], t.[ResponsibleRole] = s.[ResponsibleRole], t.[EscalateToRole] = s.[EscalateToRole], t.[EscalationWindow] = s.[EscalationWindow]
WHEN NOT MATCHED THEN INSERT ([Id], [TaskListId], [Text], [SortOrder], [IsTimeBoxed], [Category], [Cadence], [ResponsibleRole], [EscalateToRole], [EscalationWindow])
    VALUES (s.[Id], s.[TaskListId], s.[Text], s.[SortOrder], s.[IsTimeBoxed], s.[Category], s.[Cadence], s.[ResponsibleRole], s.[EscalateToRole], s.[EscalationWindow]);
SET IDENTITY_INSERT [TaskItems] OFF;
");
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [TaskCheckpoints] ON;
MERGE [TaskCheckpoints] AS t
USING (VALUES
    (17, 53, N'Hour 1', 0),
    (18, 53, N'Hour 2', 1),
    (19, 53, N'Hour 3', 2),
    (20, 53, N'Hour 4', 3),
    (21, 53, N'Hour 5', 4),
    (22, 53, N'Hour 6', 5),
    (23, 53, N'Hour 7', 6),
    (24, 53, N'Hour 8', 7),
    (25, 54, N'Hour 1', 0),
    (26, 54, N'Hour 2', 1),
    (27, 54, N'Hour 3', 2),
    (28, 54, N'Hour 4', 3),
    (29, 54, N'Hour 5', 4),
    (30, 54, N'Hour 6', 5),
    (31, 54, N'Hour 7', 6),
    (32, 54, N'Hour 8', 7),
    (33, 55, N'Hour 1', 0),
    (34, 55, N'Hour 2', 1),
    (35, 55, N'Hour 3', 2),
    (36, 55, N'Hour 4', 3),
    (37, 55, N'Hour 5', 4),
    (38, 55, N'Hour 6', 5),
    (39, 55, N'Hour 7', 6),
    (40, 55, N'Hour 8', 7),
    (41, 56, N'Hour 1', 0),
    (42, 56, N'Hour 2', 1),
    (43, 56, N'Hour 3', 2),
    (44, 56, N'Hour 4', 3),
    (45, 56, N'Hour 5', 4),
    (46, 56, N'Hour 6', 5),
    (47, 56, N'Hour 7', 6),
    (48, 56, N'Hour 8', 7),
    (49, 57, N'Hour 1', 0),
    (50, 57, N'Hour 2', 1),
    (51, 57, N'Hour 3', 2),
    (52, 57, N'Hour 4', 3),
    (53, 57, N'Hour 5', 4),
    (54, 57, N'Hour 6', 5),
    (55, 57, N'Hour 7', 6),
    (56, 57, N'Hour 8', 7),
    (57, 58, N'Hour 1', 0),
    (58, 58, N'Hour 2', 1),
    (59, 58, N'Hour 3', 2),
    (60, 58, N'Hour 4', 3),
    (61, 58, N'Hour 5', 4),
    (62, 58, N'Hour 6', 5),
    (63, 58, N'Hour 7', 6),
    (64, 58, N'Hour 8', 7),
    (65, 59, N'Hour 1', 0),
    (66, 59, N'Hour 2', 1),
    (67, 59, N'Hour 3', 2),
    (68, 59, N'Hour 4', 3),
    (69, 59, N'Hour 5', 4),
    (70, 59, N'Hour 6', 5),
    (71, 59, N'Hour 7', 6),
    (72, 59, N'Hour 8', 7),
    (73, 60, N'Hour 1', 0),
    (74, 60, N'Hour 2', 1),
    (75, 60, N'Hour 3', 2),
    (76, 60, N'Hour 4', 3),
    (77, 60, N'Hour 5', 4),
    (78, 60, N'Hour 6', 5),
    (79, 60, N'Hour 7', 6),
    (80, 60, N'Hour 8', 7),
    (81, 77, N'Hour 1', 0),
    (82, 77, N'Hour 2', 1),
    (83, 77, N'Hour 3', 2),
    (84, 77, N'Hour 4', 3),
    (85, 77, N'Hour 5', 4),
    (86, 77, N'Hour 6', 5),
    (87, 77, N'Hour 7', 6),
    (88, 77, N'Hour 8', 7),
    (89, 78, N'Hour 1', 0),
    (90, 78, N'Hour 2', 1),
    (91, 78, N'Hour 3', 2),
    (92, 78, N'Hour 4', 3),
    (93, 78, N'Hour 5', 4),
    (94, 78, N'Hour 6', 5),
    (95, 78, N'Hour 7', 6),
    (96, 78, N'Hour 8', 7),
    (97, 79, N'Hour 1', 0),
    (98, 79, N'Hour 2', 1),
    (99, 79, N'Hour 3', 2),
    (100, 79, N'Hour 4', 3),
    (101, 79, N'Hour 5', 4),
    (102, 79, N'Hour 6', 5),
    (103, 79, N'Hour 7', 6),
    (104, 79, N'Hour 8', 7),
    (105, 80, N'Hour 1', 0),
    (106, 80, N'Hour 2', 1),
    (107, 80, N'Hour 3', 2),
    (108, 80, N'Hour 4', 3),
    (109, 80, N'Hour 5', 4),
    (110, 80, N'Hour 6', 5),
    (111, 80, N'Hour 7', 6),
    (112, 80, N'Hour 8', 7),
    (113, 81, N'Hour 1', 0),
    (114, 81, N'Hour 2', 1),
    (115, 81, N'Hour 3', 2),
    (116, 81, N'Hour 4', 3),
    (117, 81, N'Hour 5', 4),
    (118, 81, N'Hour 6', 5),
    (119, 81, N'Hour 7', 6),
    (120, 81, N'Hour 8', 7),
    (121, 82, N'Hour 1', 0),
    (122, 82, N'Hour 2', 1),
    (123, 82, N'Hour 3', 2),
    (124, 82, N'Hour 4', 3),
    (125, 82, N'Hour 5', 4),
    (126, 82, N'Hour 6', 5),
    (127, 82, N'Hour 7', 6),
    (128, 82, N'Hour 8', 7),
    (129, 83, N'Hour 1', 0),
    (130, 83, N'Hour 2', 1),
    (131, 83, N'Hour 3', 2),
    (132, 83, N'Hour 4', 3),
    (133, 83, N'Hour 5', 4),
    (134, 83, N'Hour 6', 5),
    (135, 83, N'Hour 7', 6),
    (136, 83, N'Hour 8', 7),
    (137, 84, N'Hour 1', 0),
    (138, 84, N'Hour 2', 1),
    (139, 84, N'Hour 3', 2),
    (140, 84, N'Hour 4', 3),
    (141, 84, N'Hour 5', 4),
    (142, 84, N'Hour 6', 5),
    (143, 84, N'Hour 7', 6),
    (144, 84, N'Hour 8', 7),
    (145, 101, N'Hour 1', 0),
    (146, 101, N'Hour 2', 1),
    (147, 101, N'Hour 3', 2),
    (148, 101, N'Hour 4', 3),
    (149, 101, N'Hour 5', 4),
    (150, 101, N'Hour 6', 5),
    (151, 101, N'Hour 7', 6),
    (152, 101, N'Hour 8', 7),
    (153, 102, N'Hour 1', 0),
    (154, 102, N'Hour 2', 1),
    (155, 102, N'Hour 3', 2),
    (156, 102, N'Hour 4', 3),
    (157, 102, N'Hour 5', 4),
    (158, 102, N'Hour 6', 5),
    (159, 102, N'Hour 7', 6),
    (160, 102, N'Hour 8', 7),
    (161, 103, N'Hour 1', 0),
    (162, 103, N'Hour 2', 1),
    (163, 103, N'Hour 3', 2),
    (164, 103, N'Hour 4', 3),
    (165, 103, N'Hour 5', 4),
    (166, 103, N'Hour 6', 5),
    (167, 103, N'Hour 7', 6),
    (168, 103, N'Hour 8', 7),
    (169, 104, N'Hour 1', 0),
    (170, 104, N'Hour 2', 1),
    (171, 104, N'Hour 3', 2),
    (172, 104, N'Hour 4', 3),
    (173, 104, N'Hour 5', 4),
    (174, 104, N'Hour 6', 5),
    (175, 104, N'Hour 7', 6),
    (176, 104, N'Hour 8', 7),
    (177, 105, N'Hour 1', 0),
    (178, 105, N'Hour 2', 1),
    (179, 105, N'Hour 3', 2),
    (180, 105, N'Hour 4', 3),
    (181, 105, N'Hour 5', 4),
    (182, 105, N'Hour 6', 5),
    (183, 105, N'Hour 7', 6),
    (184, 105, N'Hour 8', 7),
    (185, 106, N'Hour 1', 0),
    (186, 106, N'Hour 2', 1),
    (187, 106, N'Hour 3', 2),
    (188, 106, N'Hour 4', 3),
    (189, 106, N'Hour 5', 4),
    (190, 106, N'Hour 6', 5),
    (191, 106, N'Hour 7', 6),
    (192, 106, N'Hour 8', 7),
    (193, 107, N'Hour 1', 0),
    (194, 107, N'Hour 2', 1),
    (195, 107, N'Hour 3', 2),
    (196, 107, N'Hour 4', 3),
    (197, 107, N'Hour 5', 4),
    (198, 107, N'Hour 6', 5),
    (199, 107, N'Hour 7', 6),
    (200, 107, N'Hour 8', 7),
    (201, 108, N'Hour 1', 0),
    (202, 108, N'Hour 2', 1),
    (203, 108, N'Hour 3', 2),
    (204, 108, N'Hour 4', 3),
    (205, 108, N'Hour 5', 4),
    (206, 108, N'Hour 6', 5),
    (207, 108, N'Hour 7', 6),
    (208, 108, N'Hour 8', 7),
    (209, 125, N'Hour 1', 0),
    (210, 125, N'Hour 2', 1),
    (211, 125, N'Hour 3', 2),
    (212, 125, N'Hour 4', 3),
    (213, 125, N'Hour 5', 4),
    (214, 125, N'Hour 6', 5),
    (215, 125, N'Hour 7', 6),
    (216, 125, N'Hour 8', 7),
    (217, 126, N'Hour 1', 0),
    (218, 126, N'Hour 2', 1),
    (219, 126, N'Hour 3', 2),
    (220, 126, N'Hour 4', 3),
    (221, 126, N'Hour 5', 4),
    (222, 126, N'Hour 6', 5),
    (223, 126, N'Hour 7', 6),
    (224, 126, N'Hour 8', 7),
    (225, 127, N'Hour 1', 0),
    (226, 127, N'Hour 2', 1),
    (227, 127, N'Hour 3', 2),
    (228, 127, N'Hour 4', 3),
    (229, 127, N'Hour 5', 4),
    (230, 127, N'Hour 6', 5),
    (231, 127, N'Hour 7', 6),
    (232, 127, N'Hour 8', 7),
    (233, 128, N'Hour 1', 0),
    (234, 128, N'Hour 2', 1),
    (235, 128, N'Hour 3', 2),
    (236, 128, N'Hour 4', 3),
    (237, 128, N'Hour 5', 4),
    (238, 128, N'Hour 6', 5),
    (239, 128, N'Hour 7', 6),
    (240, 128, N'Hour 8', 7),
    (241, 129, N'Hour 1', 0),
    (242, 129, N'Hour 2', 1),
    (243, 129, N'Hour 3', 2),
    (244, 129, N'Hour 4', 3),
    (245, 129, N'Hour 5', 4),
    (246, 129, N'Hour 6', 5),
    (247, 129, N'Hour 7', 6),
    (248, 129, N'Hour 8', 7),
    (249, 130, N'Hour 1', 0),
    (250, 130, N'Hour 2', 1),
    (251, 130, N'Hour 3', 2),
    (252, 130, N'Hour 4', 3),
    (253, 130, N'Hour 5', 4),
    (254, 130, N'Hour 6', 5),
    (255, 130, N'Hour 7', 6),
    (256, 130, N'Hour 8', 7),
    (257, 131, N'Hour 1', 0),
    (258, 131, N'Hour 2', 1),
    (259, 131, N'Hour 3', 2),
    (260, 131, N'Hour 4', 3),
    (261, 131, N'Hour 5', 4),
    (262, 131, N'Hour 6', 5),
    (263, 131, N'Hour 7', 6),
    (264, 131, N'Hour 8', 7),
    (265, 132, N'Hour 1', 0),
    (266, 132, N'Hour 2', 1),
    (267, 132, N'Hour 3', 2),
    (268, 132, N'Hour 4', 3),
    (269, 132, N'Hour 5', 4),
    (270, 132, N'Hour 6', 5),
    (271, 132, N'Hour 7', 6),
    (272, 132, N'Hour 8', 7)
) AS s([Id], [TaskItemId], [Label], [SortOrder])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[TaskItemId] = s.[TaskItemId], t.[Label] = s.[Label], t.[SortOrder] = s.[SortOrder]
WHEN NOT MATCHED THEN INSERT ([Id], [TaskItemId], [Label], [SortOrder])
    VALUES (s.[Id], s.[TaskItemId], s.[Label], s.[SortOrder]);
SET IDENTITY_INSERT [TaskCheckpoints] OFF;
");
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [RosterPeople] ON;
MERGE [RosterPeople] AS t
USING (VALUES
    (1, N'Hod', N'George Thompson', 1, 1),
    (2, N'Hod', N'Lukasz Jaworski', 2, 1),
    (3, N'Hod', N'Alison Gilley', 3, 1),
    (4, N'Hod', N'Piotr Pelka', 4, 1),
    (5, N'Hod', N'Michael Tregillis', 5, 1)
) AS s([Id], [ListKind], [Name], [SortOrder], [IsActive])
ON t.[Id] = s.[Id]
WHEN MATCHED THEN UPDATE SET t.[ListKind] = s.[ListKind], t.[Name] = s.[Name], t.[SortOrder] = s.[SortOrder], t.[IsActive] = s.[IsActive]
WHEN NOT MATCHED THEN INSERT ([Id], [ListKind], [Name], [SortOrder], [IsActive])
    VALUES (s.[Id], s.[ListKind], s.[Name], s.[SortOrder], s.[IsActive]);
SET IDENTITY_INSERT [RosterPeople] OFF;
");
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
