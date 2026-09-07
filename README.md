# Support Audit System

An ASP.NET Core 8 Razor Pages application for the support departments'
daily **Safe Start** checklists (reference **SHEF014**). It replaces the paper
checklists Stores completes every shift, and is built so that Dispatch and later
Logistics are pure configuration — no department, area, shift or task text lives
in code.

It shares the exact visual identity of the Production Audit System (RittalTLSW)
but has its own SQL Server database (**RittalSupportSW** on CSMSVR02) and its own
IIS site.

## The governing rule

Every department, area, shift and task line is a database row, editable through
the admin screens. The whole of Stores could be deleted and re-entered through
admin with no code change and no redeployment.

## Domain

- **Department** → **Area** + **Shift** (both belong to a department).
- **TaskList** — the task lines for one Area + Shift, **versioned**. Editing a
  list that has been filled against forks a new version; completed checklists
  stay pinned to the version they were signed on, so a reprint matches exactly.
- **TaskItem** — one line, answered **Done / Issue** with a note. A time-boxed
  line (floors & fire exits at 9am/11am/1pm/3pm; parcels every two hours) is one
  task with named **checkpoints**, each ticked and timestamped separately.
- **ChecklistSubmission** — a filled checklist, resolved by **find-or-create on
  (Area, Shift, Date) only** — never on the person's name, so two people on a
  shift share one form.

Two unique indexes guard the configuration: an area name is unique within its
department, and a shift name is unique within its department.

## Screens

- **Home** — pick department, area, shift and date; opens (or reopens) the day's
  checklist.
- **Checklist entry** — phone / iPad / laptop friendly. Single column, one task
  per card, Done/Issue as large side-by-side targets, notes expanding under an
  Issue, a running progress indicator, and **each tap saved immediately**.
  Completion is blocked while any Issue has no note.
- **Completed** — filterable by department, area, shift and date range, with an
  issue count per row.
- **Admin** — full CRUD across departments, areas, shifts and task lists, with
  easy reordering of task items.
- **PDF export** (QuestPDF) — reproduces the paper layout from the pinned task
  list version: health & reps reminder line, auditor names, date/time, location,
  the task table with responses and notes, and SHEF014 in the footer.

## Seed data

A data migration (`SeedSupportData`) creates Stores and Dispatch; for Stores the
areas *Stores DP1 & DP3* (one combined area — the paper form covers both) and
*Consumables*, the four shifts, and the four task lists transcribed verbatim from
the Word documents (15 / 15 / 12 / 10 lines). Consumables gets empty lists across
all four shifts; Dispatch gets the department row only. Everything the 1st-shift
form recorded as Done/Issue/Notes and the others as Y/N is standardised on
Done/Issue plus Notes, and the 2nd-shift Y/N reversal on two rows is not
reproduced.

## Auth & hosting

Windows Authentication via IIS — the app reads the identity IIS forwards rather
than configuring authentication in code. See `deploy/README.md` for the IIS site
setup on csm-srv-16.

## Running locally

```
dotnet ef database update      # or let startup migrate
dotnet run
```

In Development, `Admin:GrantAll` opens all admin screens.
