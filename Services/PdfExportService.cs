using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SupportAuditSystem.Models;

namespace SupportAuditSystem.Services
{
    // Reproduces the paper Safe Start checklist (reference SHEF014) from the
    // pinned task-list version, so a reprint matches what was signed. Layout
    // mirrors the Production Audit System's PDF styling.
    public class PdfExportService
    {
        private const string Red = "#CC1F2C";
        private const string DarkGray = "#1a1a1a";
        private const string MidGray = "#6b7280";
        private const string LightGray = "#f3f4f6";
        private const string BorderGray = "#e5e7eb";
        private const string GreenBg = "#d1fae5";
        private const string GreenText = "#065f46";
        private const string RedBg = "#fee2e2";
        private const string RedText = "#991b1b";

        // Standard reminder printed when a task-list version carries none.
        public const string DefaultReminder =
            "Please remember to check on the health and wellbeing of your team throughout the shift, " +
            "and make sure you know who your Health & Safety Representatives are.";

        public byte[] GenerateChecklist(ChecklistSubmission s)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var department = s.Area?.Department?.Name ?? "";
            var areaName = s.Area?.Name ?? "";
            var shiftName = s.Shift?.Name ?? "";
            var items = (s.TaskList?.Items ?? new()).OrderBy(i => i.SortOrder).ToList();
            var responsesByItem = s.Responses.ToDictionary(r => r.TaskItemId);
            var reminder = string.IsNullOrWhiteSpace(s.TaskList?.HealthRepsReminder)
                ? DefaultReminder : s.TaskList!.HealthRepsReminder!;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(36);
                    page.MarginTop(28);
                    page.MarginBottom(28);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    // ── Header ──────────────────────────────────────────────
                    page.Header().BorderBottom(2).BorderColor(Red).PaddingBottom(10).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("RITTAL").FontSize(18).Bold().FontColor(DarkGray);
                            c.Item().Text($"Daily Safe Start Checklist — {department}").FontSize(10).FontColor(MidGray);
                            c.Item().Text($"{areaName} · {shiftName} shift").FontSize(9).FontColor(MidGray);
                        });
                        row.ConstantItem(6).Background(Red);
                    });

                    page.Content().PaddingTop(12).Column(col =>
                    {
                        col.Spacing(8);

                        // Health & reps reminder line — reproduced from the paper form.
                        col.Item().Background("#fef2f2").Border(0.5f).BorderColor("#fecaca").Padding(8)
                            .Text(reminder).FontSize(8).Italic().FontColor("#7f1d1d");

                        // Details strip.
                        col.Item().Background(LightGray).Border(0.5f).BorderColor(BorderGray).Padding(10).Table(t =>
                        {
                            t.ColumnsDefinition(cd => { cd.RelativeColumn(); cd.RelativeColumn(); cd.RelativeColumn(); cd.RelativeColumn(); });
                            LabelCell(t, "Auditor Names"); ValueCell(t, s.AuditorNames ?? "—");
                            LabelCell(t, "Location"); ValueCell(t, s.Location ?? "—");
                            LabelCell(t, "Date"); ValueCell(t, s.ChecklistDate.ToString("dd/MM/yyyy"));
                            LabelCell(t, "Completed");
                            ValueCell(t, s.CompletedAt.HasValue
                                ? s.CompletedAt.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                                : "Not yet completed");
                        });

                        // Task table.
                        col.Item().Border(0.5f).BorderColor(BorderGray).Table(t =>
                        {
                            t.ColumnsDefinition(cd =>
                            {
                                cd.RelativeColumn(5);   // task
                                cd.ConstantColumn(70);  // response
                                cd.RelativeColumn(3);   // notes
                            });

                            t.Header(h =>
                            {
                                h.Cell().Background(DarkGray).Padding(5).Text("Task").FontColor("#fff").Bold().FontSize(8);
                                h.Cell().Background(DarkGray).Padding(5).AlignCenter().Text("Response").FontColor("#fff").Bold().FontSize(8);
                                h.Cell().Background(DarkGray).Padding(5).Text("Notes").FontColor("#fff").Bold().FontSize(8);
                            });

                            foreach (var item in items)
                            {
                                responsesByItem.TryGetValue(item.Id, out var resp);

                                t.Cell().Border(0.5f).BorderColor(BorderGray).Padding(5).Column(cc =>
                                {
                                    cc.Item().Text(item.Text).FontSize(8);
                                    if (item.IsTimeBoxed && resp != null)
                                    {
                                        var byCp = resp.CheckpointResponses.ToDictionary(x => x.TaskCheckpointId);
                                        cc.Item().PaddingTop(3).Text(txt =>
                                        {
                                            foreach (var cp in item.Checkpoints.OrderBy(x => x.SortOrder))
                                            {
                                                byCp.TryGetValue(cp.Id, out var cr);
                                                var tick = cr != null && cr.Ticked
                                                    ? $"{cp.Label} {cr.TickedAt?.ToLocalTime():HH:mm}"
                                                    : $"{cp.Label} —";
                                                txt.Span("  " + tick + "  ").FontSize(7)
                                                   .FontColor(cr != null && cr.Ticked ? GreenText : MidGray);
                                            }
                                        });
                                    }
                                });

                                t.Cell().Border(0.5f).BorderColor(BorderGray).AlignCenter().AlignMiddle().Element(e =>
                                {
                                    StatusChip(e, resp?.Status);
                                });

                                t.Cell().Border(0.5f).BorderColor(BorderGray).Padding(5)
                                    .Text(string.IsNullOrWhiteSpace(resp?.Notes) ? "" : resp!.Notes).FontSize(8);
                            }
                        });
                    });

                    // ── Footer — SHEF014 ────────────────────────────────────
                    page.Footer().PaddingTop(6).Row(row =>
                    {
                        row.RelativeItem().Text("SHEF014").FontSize(8).Bold().FontColor(MidGray);
                        row.RelativeItem().AlignRight().Text(t =>
                            t.Span($"Support Audit System — printed {DateTime.Now:dd MMM yyyy HH:mm}").FontSize(8).FontColor(MidGray));
                    });
                });
            }).GeneratePdf();
        }

        private static void LabelCell(TableDescriptor t, string label) =>
            t.Cell().PaddingVertical(2).PaddingRight(8).Text(label).FontColor(MidGray).FontSize(8);

        private static void ValueCell(TableDescriptor t, string value) =>
            t.Cell().PaddingVertical(2).Text(value).Bold().FontSize(8);

        private static void StatusChip(IContainer e, string? status)
        {
            var (bg, fg, label) = status switch
            {
                Models.TaskStatus.Done => (GreenBg, GreenText, "Done"),
                Models.TaskStatus.Issue => (RedBg, RedText, "Issue"),
                _ => (LightGray, MidGray, "—"),
            };
            e.Padding(3).Background(bg).Padding(3).AlignCenter().Text(label).FontColor(fg).Bold().FontSize(8);
        }
    }
}
