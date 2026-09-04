# TopSpecs

## Projektüberblick
.NET REST API zur generischen Verwaltung von Gegenständen und deren
Spezifikationen (Assets, Components, Specs), mit KI-optimiertem Sharing über
ShareLinks. Details, Domänenmodell und alle Architekturentscheidungen:
siehe `docs/TopSpecs-arc42-V1.md` (verbindlich, wird bei Architektur-Änderungen
zuerst aktualisiert).

## Tech-Stack
.NET · ASP.NET Core Minimal APIs · EF Core + PostgreSQL (JSONB) · Angular ·
Keycloak · Docker/Aspire · FluentValidation · MediatR

## Architektur-Regeln (verbindlich)
- Schichten: `Presentation → Infrastructure → UseCases/Domain → SharedKernel`.
  `Domain` kennt keine andere Schicht.
- Zwei Bounded Contexts: `Inventory` und `Templates`. `Inventory` referenziert
  aus `Templates` **nur** die ID-Typen (`AssetTemplateId`, `ComponentTemplateId`,
  `SpecTemplateId`) – nie Aggregate oder Business-Logik.
- CQRS via MediatR, Vertical Slices: ein Ordner pro Feature unter
  `UseCases/<Kontext>/<Aggregat>/<Feature>/`.
- Result-Pattern statt Exceptions für erwartbare Business-Fehler.
- ValueObjects statt Primitives, **keine Enums**: geschlossene Wertemenge →
  `private const string` + Factory-Methods (z. B. `Ownership`); offene
  Wertemenge → validierende Hülle (z. B. `Unit`).
- Specification-Pattern-Klassen **immer ausgeschrieben** (`...Specification`),
  nie zu `...Spec` abkürzen – Verwechslung mit der Entity `Spec`.
- Domain-IDs typsicher (`AssetId` statt `Guid`).
- Primary Constructors (C# 12) in Handlern.
- Hybrid Validation: Handler = Cross-Entity/Cross-Context, Entity = Domain-Invarianten.
- Jedes Entity erbt `AuditInfo` über `EntityBase<TId>` – automatisch befüllt
  per EF-Core-`SaveChangesInterceptor`, niemals manuell im Handler setzen.
- Löschen ist immer Soft-Delete (`AuditInfo.OnDelete(...)` + globaler
  Query-Filter), kein `DbSet.Remove()`.
- Mutationen an einem verschachtelten `Component` laufen ausschließlich über
  die Aggregatwurzel `Asset`, per `targetComponentId`-Parameter
  (`null` = wirkt auf das Asset selbst), z. B. `AddSpec`, `SetOwnership`,
  `SetLifecycleStatus`, `SetDescription`.

## Befehle
```
dotnet build
dotnet test
dotnet run --project src/4-Aspire/TopSpecs.AppHost
```

## Bei Architektur-Änderungen
Neue Entscheidungen zuerst als ADR-Eintrag in `docs/TopSpecs-arc42-V1.md`
(Kapitel 9) festhalten, dann erst umsetzen – nicht nur im Code kommentieren.
