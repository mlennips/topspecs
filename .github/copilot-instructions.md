# Copilot-Anweisungen für TopSpecs

Kanonische Quelle für Architektur-Regeln, Tech-Stack und Konventionen:
`CLAUDE.md` im Repo-Root (@CLAUDE.md). Bei Änderungen dort zuerst aktualisieren,
diese Datei nur nachziehen.

## Kurzfassung (Fallback, falls der Include oben nicht aufgelöst wird)

**Tech-Stack:** .NET · ASP.NET Core Minimal APIs · EF Core + PostgreSQL (JSONB) ·
Blazor WebAssembly (MudBlazor) · Keycloak · Docker/Aspire · FluentValidation · MediatR

**Architektur-Regeln:**
- Schichten: `Presentation → Infrastructure → UseCases/Domain → SharedKernel`.
  `Domain` kennt keine andere Schicht.
- Zwei Bounded Contexts: `Inventory` und `Templates`. `Inventory` referenziert
  aus `Templates` **nur** die ID-Typen – nie Aggregate oder Business-Logik.
- CQRS via MediatR, Vertical Slices: ein Ordner pro Feature unter
  `UseCases/<Kontext>/<Aggregat>/<Feature>/`.
- Result-Pattern statt Exceptions für erwartbare Business-Fehler.
- ValueObjects statt Primitives, **keine Enums**.
- Specification-Pattern-Klassen **immer ausgeschrieben** (`...Specification`),
  nie zu `...Spec` abkürzen.
- Domain-IDs typsicher (`AssetId` statt `Guid`).
- Primary Constructors (C# 12) in Handlern.
- Jedes Entity trägt `AuditInfo` (automatisch per Interceptor, nie manuell).
- Löschen ist immer Soft-Delete, kein physisches Entfernen.
- Component-Mutationen laufen über `Asset` mit `targetComponentId`-Parameter,
  nie direkt am `Component`.

## Quellcode-Qualitätsstandards
- Definiere Quellcode-Qualitätsstandards, die Verstöße direkt in IDE und Compiler sichtbar machen. 

Details, Domänenmodell, ADRs: siehe `docs/TopSpecs-arc42-V1.md`.
