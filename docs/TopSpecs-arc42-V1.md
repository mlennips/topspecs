# TopSpecs – Architekturdokumentation (arc42)

**Version: V1**

---

## 1. Einführung und Ziele

### 1.1 Aufgabenstellung

Für viele KI-Chats wird wiederholt fachlicher Kontext zu unterschiedlichen
Gegenständen benötigt (z. B. PV-Anlage, Rennrad, Computer). Dieser Kontext liegt
aktuell verstreut in Dateien vor. **TopSpecs** verwaltet diesen Kontext strukturiert
und generisch – nicht auf einen Gegenstandstyp festgelegt – und macht ihn in
unterschiedlichen Formaten teilbar, insbesondere für KI-Konsum optimiert.

Kernfähigkeiten:

- Beliebige Gegenstände (`Asset`) mit beliebig tief verschachtelten Bauteilen
  (`Component`) und Spezifikationen (`Spec`) erfassen, gruppiert nach
  Themenbereichen (`Bubble`). Specs können sowohl direkt am `Asset` als auch an
  einem `Component` hängen.
- Wiederverwendbare Vorlagen (`AssetTemplate`, `ComponentTemplate`,
  `SpecTemplate`) für wiederkehrende Gegenstands-Arten.
- Anschaffungsstatus (`Ownership`: Owned/Planning/Wishlist) auf Asset- **und**
  Component-Ebene unabhängig nachhalten.
- Reale Identität (`AssetIdentity`: Seriennummer/Asset-Tag) für den eigentlichen
  „Zwilling"-Bezug zu einem physischen Exemplar.
- Freitext- und Messwert-Verlauf (`Journal`) pro Asset/Component – z. B.
  Werkstatt-Ergebnisse, Kilometerstand, Beobachtungsnotizen.
- Dauerhafte Beschreibung (`Description`) auf allen Kern-Ebenen beider
  Kontexte – abgegrenzt vom datierten `Journal`.
- Automatische Nachvollziehbarkeit (`AuditInfo`: erstellt/geändert/gelöscht,
  je mit Zeitpunkt und Nutzer) auf **jedem** Entity beider Kontexte,
  inklusive Soft-Delete statt physischem Löschen.
- Fachliche Beziehungen zwischen unabhängigen Assets/Components
  (`AssetRelationship`, z. B. „Wechselrichter versorgt Wallbox").
- Lebenszyklus-Status (`LifecycleStatus`, z. B. Active/InRepair/Sold/Disposed)
  auf Asset- **und** Component-Ebene, unabhängig von `Ownership`.
- Anhänge (`Attachment`: Fotos, Handbücher, Rechnungen) auf Asset-, Component-
  **und** Spec-Ebene.
- Standort (`Location`) auf Asset-Ebene.
- Zeitlich befristete, formatspezifische Freigabelinks (`ShareLink`) pro Asset.
- Optionale Shop-/Affiliate-Links (`PurchaseLink`) pro Asset/Component.
- *(Geplant, erste Ausbaustufe grob skizziert)* KI-Anbindung via **MCP**
  (Model Context Protocol): fremde KI-Clients greifen strukturiert auf ein
  freigegebenes Asset zu, authentifiziert über denselben `ShareLink`-Token –
  kein separates API-Key-Konzept. Details folgen in einer späteren Version.

### 1.2 Qualitätsziele (Top 5)

| Rang | Qualitätsziel | Motivation |
|---|---|---|
| 1 | **Erweiterbarkeit** | Neue Asset-Arten/Spec-Typen ohne Codeänderung am Kern (Templates statt Vererbung) |
| 2 | **Wartbarkeit** | Klare Schichtentrennung (Clean Architecture) + Vertical Slices + saubere Bounded-Context-Grenze zwischen Inventory und Templates |
| 3 | **Interoperabilität** | ShareLink-Export muss sich sauber in KI-Chat-Workflows einfügen (JSON, KI-optimiertes Markdown) |
| 4 | **Datenschutz/Sicherheit** | ShareLinks sind unauthentifiziert erreichbar → Ablaufdatum, Widerruf |
| 5 | **Testbarkeit** | Domänenlogik (Invarianten, Template-Anwendung, Rekursion) isoliert und ohne Infrastruktur testbar |

### 1.3 App-Name

**TopSpecs** – final festgelegt.

### 1.4 Stakeholder

| Rolle | Erwartung |
|---|---|
| Nutzer (Betreiber = Entwickler selbst) | Schnelle Pflege eigener Assets, verlässliches Teilen mit KI-Tools |
| Weiterentwickler (zukünftig) | Nachvollziehbare Architektur, generisch erweiterbar |
| KI-Chat (indirekter Konsument) | Wohlstrukturierter, kompakter Kontext über ShareLink-Ausgabe |

---

## 2. Randbedingungen

### 2.1 Technische Randbedingungen

- Backend: **.NET / C#** (Primary Constructors, Records, init-only Properties)
- **ASP.NET Core** (Minimal APIs, OpenAPI)
- **Entity Framework Core** mit **PostgreSQL**, dynamische Specs via **JSONB**
- Frontend: **Angular**
- Auth: **Keycloak**
- Deployment: **Docker / .NET Aspire**

### 2.2 Architektur-Randbedingungen

- **Clean Architecture** mit **Vertical Slices** (Feature-fokussierte
  Ordnerstruktur innerhalb der UseCases-Schicht)
- **DDD mit zwei Bounded Contexts**: `Inventory` und `Templates` (siehe Kapitel 5)
- **CQRS/CQS**: Commands (write) und Queries (read) strikt getrennt
- **Unit of Work** für Transaktionssteuerung über mehrere Repository-Operationen
- **Repository Pattern** mit Write/Read-Trennung (`IWriteRepository<T>` /
  `IReadRepository<T>`)
- **Specification Pattern** für wiederverwendbare, komponierbare Query-Filter –
  Klassen werden **immer ausgeschrieben** (`...Specification`), nie zu `...Spec`
  abgekürzt, um Verwechslung mit der Domänen-Entity `Spec` zu vermeiden
- **Result Pattern**: keine Exceptions für erwartbare Business-Fehler
- **ValueObjects statt Primitives**, **keine Enums** – geschlossene Wertemengen
  (z. B. `Ownership`) über string-basierte Konstanten mit Factory-Methods,
  offene Wertemengen (z. B. `Unit`) über validierende ValueObject-Hüllen
- **Hybrid Validation**: Handler = Cross-Entity-Validierung, Entity = Domain-Invarianten
- **Primary Constructors** (C# 12) für Dependency Injection in Handlern

### 2.3 Organisatorische Randbedingungen

- Einzelentwickler-Projekt, iterative Entwicklung, kein Big-Bang-Release.

### 2.4 Konventionen

- Code, Domänenbegriffe, Kommentare: **Englisch**
- Dokumentation, Commit-Beschreibungen: **Deutsch**
- Backend liefert nur Keys/Strings (z. B. `"Owned"`, `"Planning"`), keine
  deutschen Texte und keine Icons/Emojis im Domänencode – Frontend übernimmt
  i18n und Icon-Mapping via CSS-Klassen auf Basis des Status-Strings

---

## 3. Kontextabgrenzung

### 3.1 Fachlicher Kontext

```
                    ┌─────────────────────┐
   pflegt Assets     │                     │   ruft ShareLink auf
   ────────────────▶ │      TopSpecs       │ ◀──────────────────────
   (Nutzer, Web-UI)   │                     │   (KI-Chat / Browser,
                      └─────────────────────┘    kein Login)
```

### 3.2 Technischer Kontext

```
Angular SPA ──HTTPS/REST/JSON──▶ ASP.NET Core API ──EF Core──▶ PostgreSQL (JSONB)
                                        │
                                   Keycloak (OIDC) – Authentifizierung Web-UI

Anonymer Client (ShareLink-Aufruf) ──HTTPS──▶ öffentlicher API-Endpunkt (kein Login)
```

---

## 4. Lösungsstrategie

| Ziel | Strategie |
|---|---|
| Erweiterbarkeit | Rekursives Domänenmodell (`Component` mit Sub-Components) statt starrer Tiefe; Fachwissen steckt in **Templates** (Daten), nicht im Code |
| Klare Verantwortlichkeiten | Trennung in zwei Bounded Contexts: `Inventory` (aktuelle Bestände) und `Templates` (wiederverwendbare Vorlagen) – Kopplung nur über typisierte IDs, keine geteilten Aggregate |
| Wartbarkeit | Clean Architecture (`Presentation → Infrastructure → UseCases/Domain → SharedKernel`) + Vertical Slices je Use Case |
| Konsistente Transaktionen | Unit of Work kapselt mehrere Repository-Operationen pro Command |
| Flexible Specs | JSONB-Spalte für dynamische Spec-Werte statt starrem relationalem Schema |
| Nachvollziehbarkeit über Zeit | `Journal` als eigenständiges, schlankes Aggregat statt Aufblähung des Asset-Aggregats |
| Testbarkeit | DDD-Aggregate kapseln Invarianten, frei von Infrastruktur; CQRS trennt Lese-/Schreibpfade |
| Interoperabilität | ShareLink-Auflösung nutzt Strategy-Pattern pro `OutputFormat`-Konstante |

---

## 5. Bausteinsicht

### 5.1 Ebene 1 – Grobstruktur

```
/src
  /1-Presentation
    TopSpecs.Api               (ASP.NET Core: Endpunkte, Keycloak-Auth, Mapping)
    TopSpecs.Web                (Angular SPA)
    TopSpecs.Mcp                 (geplant, nicht ausdetailliert – MCP-Tools,
                                   Auth über ShareLink-Token statt eigenem API-Key)
  /2-Core
    TopSpecs.Domain
      ├─ Inventory/              (Bubble, Asset, Component, Spec, ShareLink,
      │                           Journal, ValueObjects)
      └─ Templates/               (AssetTemplate, ComponentTemplate, SpecTemplate, VOs)
    TopSpecs.UseCases
      ├─ Inventory/               (Vertical Slices: Bubbles/, Assets/, ShareLinks/,
      │                            Journals/)
      └─ Templates/                (Vertical Slices: AssetTemplates/, ComponentTemplates/, SpecTemplates/)
    TopSpecs.SharedKernel          (EntityBase, AuditInfo, UserId, AggregateRoot,
                                     Entity, Result, Guard, Specification<T>)
  /3-Infrastructure
    TopSpecs.Infrastructure         (EF Core, Read-/Write-Repositories, Unit of Work,
                                      Keycloak-Integration, Output-Formatter)
  /4-Aspire
    TopSpecs.AppHost                 (Orchestrierung: Api, Web, Postgres, Keycloak)
    TopSpecs.ServiceDefaults          (Telemetry, Health Checks, Resilience)
/tests                                 (eigene Ebene, kein Layer unter /src – Tests
                                         stehen quer zu allen Architektur-Schichten)
  SharedKernel.UnitTests
  Domain.Inventory.UnitTests
  Domain.Templates.UnitTests
  UseCases.UnitTests
  Infrastructure.IntegrationTests
  Api.FunctionalTests
  Architecture.Tests
```

### 5.2 Fundament: `EntityBase` und `AuditInfo` (SharedKernel)

Gilt kontextübergreifend für **jedes** Entity in beiden Bounded Contexts –
kein Opt-in-Interface wie `IHasSpecs`/`IHasDescription`, sondern Teil der
gemeinsamen Basisklasse, von der `AggregateRoot<TId>` und `Entity<TId>`
ableiten.

```csharp
namespace TopSpecs.SharedKernel;

public interface IEntityBase
{
    AuditInfo Audit { get; }
}

public abstract class EntityBase<TId> : IEntityBase
{
    public TId Id { get; protected set; } = default!;
    public AuditInfo Audit { get; internal set; } = default!;   // von Infrastructure gesetzt
}

public sealed class AuditInfo : ValueObject
{
    public DateTimeOffset CreatedAt { get; }
    public UserId CreatedBy { get; }
    public DateTimeOffset? UpdatedAt { get; }
    public UserId? UpdatedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public UserId? DeletedBy { get; }
    public bool IsDeleted => DeletedAt is not null;             // rein abgeleitet

    public static AuditInfo OnCreate(UserId createdBy, DateTimeOffset now)
        => new(now, createdBy, null, null, null, null);
    public AuditInfo OnUpdate(UserId updatedBy, DateTimeOffset now)
        => new(CreatedAt, CreatedBy, now, updatedBy, DeletedAt, DeletedBy);
    public AuditInfo OnDelete(UserId deletedBy, DateTimeOffset now)
        => new(CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, now, deletedBy);

    private AuditInfo(DateTimeOffset createdAt, UserId createdBy, DateTimeOffset? updatedAt,
        UserId? updatedBy, DateTimeOffset? deletedAt, UserId? deletedBy)
        => (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy)
         = (createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy);
}

public sealed class UserId : ValueObject
{
    // Wrapt die Keycloak-Subject-ID (sub-Claim) – kein eigenes User-Aggregat
    // im Domänenmodell nötig, Keycloak übernimmt die Nutzerverwaltung.
    public string Value { get; }
    public static UserId Of(string keycloakSubject) => new(keycloakSubject);
}

// Konkrete Basisklassen für Aggregate Roots und Kind-Entities – beide erben
// AuditInfo transitiv über EntityBase<TId>, ohne es erneut zu deklarieren.
public abstract class AggregateRoot<TId> : EntityBase<TId> { }
public abstract class Entity<TId> : EntityBase<TId> { }
```

**Automatische Befüllung** (nicht Handler/Entity, sondern `Infrastructure`):

```csharp
public sealed class AuditInfoInterceptor(ICurrentUserProvider currentUser, TimeProvider clock) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var now = clock.GetUtcNow();
        var userId = currentUser.UserId;   // aus Keycloak-Claims der aktuellen Anfrage

        foreach (var entry in eventData.Context!.ChangeTracker.Entries<IEntityBase>())
        {
            entry.Entity.Audit = entry.State switch
            {
                EntityState.Added    => AuditInfo.OnCreate(userId, now),
                EntityState.Modified => entry.Entity.Audit.OnUpdate(userId, now),
                _ => entry.Entity.Audit
            };
        }
        return base.SavingChanges(eventData, result);
    }
}
```

Läuft für **alle** Entities im `DbContext` (Aggregate Roots und Kind-Entities
wie `Component`, `Spec`, `JournalEntry` gleichermaßen), da EF Core sie auch
als eigenständige Change-Tracking-Einträge führt.

**Soft-Delete statt Hard-Delete:** `IWriteRepository<T>.DeleteAsync` ruft eine
Domain-Methode auf (z. B. `asset.Delete(userId, now)`), die `AuditInfo.OnDelete(...)`
setzt – die Zeile bleibt in der Datenbank, wird aber per **globalem
Query-Filter** (`HasQueryFilter(e => !e.Audit.IsDeleted)` in der EF-Core-
Konfiguration) aus normalen Abfragen ausgeblendet.

**Nebenläufigkeit:** Optimistic-Concurrency-Token über PostgreSQLs
`xmin`-Systemspalte (`modelBuilder.Entity<T>().UseXminAsConcurrencyToken()`,
nativ von Npgsql unterstützt) – **reine Infrastructure-Konfiguration**, kein
zusätzliches Domain-Property nötig, verhindert stillschweigend überschriebene
gleichzeitige Änderungen.

### 5.3 Ebene 2 – Bounded Context „Inventory"

| Aggregat (Root) | Enthält | Verantwortung |
|---|---|---|
| `Bubble` | – | Themenbereich, dem Assets zugeordnet werden (z. B. „Fahrräder", „PC") |
| `Asset` | `Component[]` (rekursiv), `Spec[]` (direkt am Asset) | Ein konkreter Gegenstand (z. B. „Rennrad") |
| `ShareLink` | – | Zeitlich befristeter, formatspezifischer Zugriff auf ein `Asset` |
| `Journal` | `JournalEntry[]` | Freitext-/Messwert-Verlauf zu einem Asset oder Component (z. B. Werkstatt-Ergebnisse) |
| `Attachment` | – | Datei-Referenz (Foto, Handbuch, Rechnung) an Asset-, Component- oder Spec-Ebene |
| `AssetRelationship` | – | Fachliche Beziehung zwischen zwei Assets/Components (z. B. „Wechselrichter versorgt Wallbox") |

`Component` und `Spec` sind **Teil** des `Asset`-Aggregats (Konsistenzgrenze).
`Journal` ist bewusst ein **eigenständiges** Aggregat, nur per Id
referenziert – es wächst unbegrenzt über die Zeit und würde das
`Asset`-Aggregat sonst bei jedem Laden unnötig aufblähen.

```csharp
namespace TopSpecs.Domain.Inventory;

public interface IHasSpecs
{
    IReadOnlyCollection<Spec> Specs { get; }
}

public interface IHasDescription
{
    // Freier Text, keine Invariante außer optionaler Länge – daher plain string,
    // keine ValueObject-Hülle (anders als Ownership/LifecycleStatus mit fester
    // bzw. validierter Wertemenge). Analog zum bestehenden Muster IHasDisplayName.
    string? Description { get; }
}

public sealed class Bubble : AggregateRoot<BubbleId>, IHasDescription
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
}

public sealed class Asset : AggregateRoot<AssetId>, IHasSpecs, IHasDescription
{
    public BubbleId BubbleId { get; private set; }
    public AssetTemplateId? TemplateId { get; private set; }   // leichte Kopplung an Templates-Kontext
    public Ownership Ownership { get; private set; }
    public LifecycleStatus LifecycleStatus { get; private set; }
    public AssetIdentity? Identity { get; private set; }
    public Location? Location { get; private set; }
    public PurchaseLink? PurchaseLink { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<Component> Components => _components.AsReadOnly();
    public IReadOnlyCollection<Spec> Specs => _specs.AsReadOnly();

    public static Result<Asset> Create(string name, BubbleId bubbleId, Ownership ownership) { /* Invarianten */ }
    public Result<Component> AddComponent(string name, ComponentId? parentComponentId = null) { /* Baum konsistent halten */ }

    // Alle vier Methoden folgen demselben Muster: targetComponentId = null wirkt
    // auf das Asset selbst, ein gesetzter Wert navigiert zum Ziel-Component
    // innerhalb des Baums – Mutation läuft ausschließlich über die Aggregatwurzel.
    // Entity-Ebene: nur Basis-Invarianten (z.B. Key nicht leer). Die Cross-Context-Prüfung
    // gegen SpecTemplate.DataType/Unit passiert vorgelagert im Handler (siehe 8.7).
    public Result<Spec> AddSpec(ComponentId? targetComponentId, string key, string value, Unit? unit, SpecTemplateId? templateId = null) { /* ... */ }
    public Result SetOwnership(ComponentId? targetComponentId, Ownership ownership) { /* ... */ }
    public Result SetLifecycleStatus(ComponentId? targetComponentId, LifecycleStatus status) { /* ... */ }
    public Result SetDescription(ComponentId? targetComponentId, string? description) { /* ... */ }
}

public sealed class Component : Entity<ComponentId>, IHasSpecs, IHasDescription
{
    public ComponentId? ParentComponentId { get; private set; }        // Selbstreferenz
    public ComponentTemplateId? TemplateId { get; private set; }        // leichte Kopplung
    public Ownership Ownership { get; private set; }
    public LifecycleStatus LifecycleStatus { get; private set; }
    public AssetIdentity? Identity { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<Component> Children => _children.AsReadOnly();
    public IReadOnlyCollection<Spec> Specs => _specs.AsReadOnly();
}

public sealed class Spec : Entity<SpecId>, IHasDescription
{
    public SpecTemplateId? TemplateId { get; private set; }             // leichte Kopplung
    public bool IsCustom => TemplateId is null;                          // rein abgeleitet, keine eigene Datenhaltung
    public string Key { get; private set; }
    public string Value { get; private set; }
    public Unit? Unit { get; private set; }
    public string? Description { get; private set; }
}

public sealed class LifecycleStatus : ValueObject
{
    // Unabhängig von Ownership: Ownership beschreibt den Anschaffungsstatus,
    // LifecycleStatus den Betriebszustand danach.
    private const string ActiveKey = "Active";
    private const string InRepairKey = "InRepair";
    private const string SoldKey = "Sold";
    private const string DisposedKey = "Disposed";
    public string Value { get; }
    public static LifecycleStatus Active() => new(ActiveKey);
    public static LifecycleStatus InRepair() => new(InRepairKey);
    public static LifecycleStatus Sold() => new(SoldKey);
    public static LifecycleStatus Disposed() => new(DisposedKey);
}

public sealed class Location : ValueObject
{
    // Nur auf Asset-Ebene. Freitext-Label reicht für die meisten Fälle,
    // Koordinaten optional für spätere Kartenansicht.
    public string? Label { get; }         // z.B. "Keller", "Garage", "Dach Süd"
    public double? Latitude { get; }
    public double? Longitude { get; }
}

public sealed class Ownership : ValueObject
{
    private const string OwnedKey = "Owned";
    private const string PlanningKey = "Planning";
    private const string WishlistKey = "Wishlist";
    public string Value { get; }
    public static Ownership Owned() => new(OwnedKey);
    public static Ownership Planning() => new(PlanningKey);
    public static Ownership Wishlist() => new(WishlistKey);
}

public sealed class AssetIdentity : ValueObject
{
    public string? SerialNumber { get; }   // herstellerseitige Seriennummer
    public string? AssetTag { get; }        // eigene Kennzeichnung, z.B. QR-Code-Inhalt
}

public sealed class Unit : ValueObject
{
    // Offene Wertemenge (im Gegensatz zu Ownership) – validierende Hülle statt fester Konstanten
    public string Value { get; }
    private Unit(string value) => Value = value;
    public static Result<Unit> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Result<Unit>.Failure("Unit darf nicht leer sein");
        return Result<Unit>.Success(new Unit(value.Trim()));
    }
}
```

### 5.4 Journal

```csharp
public sealed class Journal : AggregateRoot<JournalId>
{
    public AssetId AssetId { get; private set; }
    public ComponentId? ComponentId { get; private set; }   // optional: Eintrag betrifft nur ein Component
    public IReadOnlyCollection<JournalEntry> Entries => _entries.AsReadOnly();

    public Result<JournalEntry> AddEntry(
        string category,                          // z.B. "Workshop", "Note", "Measurement" – frei erweiterbar
        string? notes,
        IReadOnlyCollection<Measurement> measurements,
        DateTimeOffset occurredAt)
    { /* Invariante: mind. Notes oder Measurements gesetzt */ }
}

public sealed class JournalEntry : Entity<JournalEntryId>
{
    public string Category { get; private set; }
    public string? Notes { get; private set; }
    public IReadOnlyCollection<Measurement> Measurements { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
}

public sealed class Measurement : ValueObject
{
    public string Key { get; private set; }     // z.B. "Kilometerstand"
    public string Value { get; private set; }
    public Unit? Unit { get; private set; }
}
```

### 5.5 AssetRelationship

Bildet fachliche Beziehungen zwischen unabhängigen Assets ab (z. B.
„Wechselrichter versorgt Wallbox" über Bubble-Grenzen hinweg) – sowohl
Asset↔Asset als auch Component↔Asset. Beide Seiten tragen symmetrisch eine
optionale `ComponentId`; dadurch wird auch Component↔Component möglich
(kein Sonderfall, sondern natürliche Konsequenz der symmetrischen Struktur).

```csharp
public sealed class AssetRelationship : AggregateRoot<AssetRelationshipId>
{
    public AssetId SourceAssetId { get; private set; }
    public ComponentId? SourceComponentId { get; private set; }   // null = ganzes Asset
    public AssetId TargetAssetId { get; private set; }
    public ComponentId? TargetComponentId { get; private set; }   // null = ganzes Asset
    public RelationType Type { get; private set; }

    public static Result<AssetRelationship> Create(
        AssetId sourceAssetId, ComponentId? sourceComponentId,
        AssetId targetAssetId, ComponentId? targetComponentId,
        RelationType type)
    {
        // Invariante: Source und Target dürfen nicht identisch sein
        // (weder auf Asset- noch auf Component-Ebene)
    }
}

public sealed class RelationType : ValueObject
{
    // Offene Wertemenge (wie Unit) – z.B. "PowersTarget", "ConnectedTo",
    // "ChargesFrom" – frei erweiterbar ohne Schema-Migration.
    public string Value { get; }
    private RelationType(string value) => Value = value;
    public static Result<RelationType> Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Result<RelationType>.Failure("RelationType darf nicht leer sein");
        return Result<RelationType>.Success(new RelationType(value.Trim()));
    }
}
```

### 5.6 Attachment

Ebenfalls ein eigenständiges Aggregat, aus demselben Grund wie `Journal`
(unbegrenztes Wachstum, kein automatisches Mitladen beim Asset-Zugriff).
Referenziert genau **eine** Ebene – Asset, Component oder Spec – über eine
Domain-Invariante erzwungen.

```csharp
public sealed class Attachment : AggregateRoot<AttachmentId>
{
    public AssetId AssetId { get; private set; }
    public ComponentId? ComponentId { get; private set; }   // gesetzt, falls Component-Ebene
    public SpecId? SpecId { get; private set; }               // gesetzt, falls Spec-Ebene
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public string StorageKey { get; private set; }             // Referenz auf Blob Storage, nicht die Datei selbst
    public long SizeBytes { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }

    public static Result<Attachment> Create(
        AssetId assetId, ComponentId? componentId, SpecId? specId,
        string fileName, string contentType, string storageKey, long sizeBytes)
    {
        // Invariante: componentId und specId nicht gleichzeitig gesetzt
        // (Spec gehört ohnehin zu genau einem Component/Asset – redundante Doppelreferenz vermeiden)
    }
}
```

Die eigentliche Datei liegt **nicht** in PostgreSQL, sondern in einem
Blob-Storage (lokal via Docker-Volume, produktiv z. B. S3-kompatibel) –
`Attachment` speichert nur Metadaten und den `StorageKey` als Referenz.

### 5.7 Ebene 2 – Bounded Context „Templates"

| Aggregat (Root) | Enthält | Verantwortung |
|---|---|---|
| `AssetTemplate` | `ComponentTemplate[]` (rekursiv), `SpecTemplate[]` | Wiederverwendbare Vorlage für eine Asset-Art (z. B. „Rennrad-Vorlage") |

```csharp
namespace TopSpecs.Domain.Templates;

public sealed class AssetTemplate : AggregateRoot<AssetTemplateId>, IHasDescription
{
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<ComponentTemplate> ComponentTemplates => _componentTemplates.AsReadOnly();
}

public sealed class ComponentTemplate : Entity<ComponentTemplateId>, IHasDescription
{
    public ComponentTemplateId? ParentComponentTemplateId { get; private set; }   // Selbstreferenz
    public string? Description { get; private set; }
    public IReadOnlyCollection<ComponentTemplate> Children => _children.AsReadOnly();
    public IReadOnlyCollection<SpecTemplate> SpecTemplates => _specTemplates.AsReadOnly();
}

public sealed class SpecTemplate : Entity<SpecTemplateId>, IHasDescription
{
    public string Key { get; private set; }
    public SpecDataType DataType { get; private set; }
    public Unit? Unit { get; private set; }
    public string? Description { get; private set; }
    public bool Required { get; private set; }
}

public sealed class SpecDataType : ValueObject
{
    // Geschlossene Wertemenge (wie Ownership/LifecycleStatus) – im Unterschied zu
    // Unit, das eine offene, frei eingebbare Wertemenge validiert.
    private const string StringKey = "string";
    private const string NumberKey = "number";
    private const string BooleanKey = "boolean";
    private const string DateKey = "date";
    public string Value { get; }
    public static SpecDataType String() => new(StringKey);
    public static SpecDataType Number() => new(NumberKey);
    public static SpecDataType Boolean() => new(BooleanKey);
    public static SpecDataType Date() => new(DateKey);
}
```

**Kopplung zwischen den Kontexten:** `Inventory` referenziert die ID-Typen
(`AssetTemplateId`, `ComponentTemplateId`, `SpecTemplateId`) direkt aus
`Templates` – **leichte Kopplung**, bewusst gewählter Mittelweg für ein
Solo-Projekt. Verboten bleibt jeder direkte Zugriff auf `Templates`-Aggregate
oder -Business-Logik von `Inventory` aus; dies wird durch einen
Architektur-Test (`NetArchTest`) erzwungen.

### 5.8 Vertical Slices in `TopSpecs.UseCases`

```
UseCases/
├─ Inventory/
│   ├─ Bubbles/       (Create/Rename/SetDescription/Delete/Get/List)
│   ├─ Assets/
│   │   ├─ CreateAsset/
│   │   ├─ CreateAssetFromTemplate/   (ruft Templates-Kontext, siehe 5.9)
│   │   ├─ AddComponent/
│   │   ├─ AddSpec/                    (an Asset oder Component, targetComponentId)
│   │   ├─ SetOwnership/                (an Asset oder Component)
│   │   ├─ SetLifecycleStatus/          (an Asset oder Component)
│   │   ├─ SetDescription/              (an Asset oder Component)
│   │   └─ GetAsset/ (+ Specifications)
│   ├─ AssetRelationships/
│   │   ├─ CreateAssetRelationship/
│   │   ├─ DeleteAssetRelationship/
│   │   └─ ListRelationshipsForAsset/
│   ├─ Journals/
│   │   ├─ AddJournalEntry/
│   │   └─ GetJournal/
│   ├─ Attachments/
│   │   ├─ UploadAttachment/            (Asset-, Component- oder Spec-Ebene)
│   │   ├─ DeleteAttachment/
│   │   └─ GetAttachmentsForTarget/
│   └─ ShareLinks/    (Create/Revoke/Resolve/GetForShareLink)
└─ Templates/
    ├─ AssetTemplates/    (Create/Update/SetDescription/List/GetSnapshot)
    ├─ ComponentTemplates/ (Create/Update/SetDescription/List)
    └─ SpecTemplates/       (Create/Update/SetDescription/List)
```

### 5.9 Cross-Context-Integration (Published Language)

`Inventory` darf beim Anlegen eines Assets aus einer Vorlage **keine**
`Templates`-Domänenobjekte direkt verwenden. Stattdessen liefert `Templates`
einen schreibgeschützten Snapshot-DTO:

```
CreateAssetFromTemplate-Slice (Inventory.UseCases)
  → ruft GetAssetTemplateSnapshotQuery (Templates.UseCases) auf
  → erhält AssetTemplateSnapshot (DTO: Name, ComponentTemplateSnapshot[], SpecTemplateSnapshot[])
  → baut daraus lokal ein neues Asset-Aggregat inkl. Component-/Spec-Baum auf
  → setzt dabei TemplateId je Ebene aus dem Snapshot
```

`Templates` „kennt" `Inventory` nicht; die einzige Schnittstelle ist dieser
eine schreibgeschützte Snapshot.

---

## 6. Laufzeitsicht

### 6.1 Szenario: Asset aus Template anlegen (kontextübergreifend)

1. Nutzer wählt ein `AssetTemplate` (z. B. „Rennrad" mit `ComponentTemplate`
   „Schaltgruppe" → „Schaltwerk").
2. `Api` → `CreateAssetFromTemplateCommand` (Slice `Inventory/Assets/CreateAssetFromTemplate`).
3. Handler ruft `GetAssetTemplateSnapshotQuery` im `Templates`-Kontext auf,
   erhält einen `AssetTemplateSnapshot` (DTO, keine Domain-Referenz).
4. Handler baut daraus rekursiv `Asset` → `Component`-Baum lokal in `Inventory`
   auf; `TemplateId` wird je Ebene aus dem Snapshot übernommen, `Ownership`
   initial `Wishlist()` oder `Planning()`. Die in 8.7 beschriebene
   Cross-Context-Validierung gegen `SpecTemplate` entfällt hier – die
   Default-Werte stammen aus derselben Vorlage, gegen die sonst geprüft würde,
   eine Prüfung wäre zirkulär. Sie greift ausschließlich, wenn ein Nutzer
   *nachträglich* einen Standard-Spec-Wert manuell ändert.
5. `IUnitOfWork.SaveChangesAsync()` persistiert den gesamten Baum als ein
   Aggregat; Specs werden in die JSONB-Spalte serialisiert.

### 6.2 Szenario: Ownership eines Sub-Components ändern

1. Nutzer setzt am Component „Schaltwerk" (Kind von „Schaltgruppe", Kind von
   Asset „Rennrad") `Ownership.Owned()`.
2. `SetOwnershipCommand` lädt das Wurzel-Aggregat `Asset` über
   `IWriteRepository<Asset>` (inkl. gesamtem Component-Baum) und ruft
   `asset.SetOwnership(targetComponentId: schaltwerkId, Ownership.Owned())`
   auf – die Navigation zum Ziel-Component passiert innerhalb der Methode,
   nicht im Handler.
3. Persistierung über Unit of Work.

### 6.3 Szenario: Werkstatt-Ergebnis im Journal erfassen

1. Nutzer trägt nach einem Werkstattbesuch am Asset „Rennrad" einen Eintrag ein:
   Notiz „Bremsen erneuert, Kette gereinigt" + Messwert „Kilometerstand: 4200 km".
2. `AddJournalEntryCommand` (Slice `Inventory/Journals/AddJournalEntry`) lädt
   (oder erstellt) das `Journal`-Aggregat für die `AssetId`.
3. `Journal.AddEntry("Workshop", "Bremsen erneuert...", [Measurement("Kilometerstand", "4200", Unit.Of("km"))], DateTimeOffset.Now)`.
4. Persistierung über Unit of Work – unabhängig vom `Asset`-Aggregat selbst.

### 6.4 Szenario: ShareLink auflösen

1. Externer Konsument ruft `GET /share/{token}` ohne Login auf.
2. `ResolveShareLinkQuery` prüft Ablauf/Widerruf, lädt das `Asset` **mit
   gesamtem Component-Baum** über eine `AssetTreeByIdSpecification`.
3. Delegation an Formatter-Strategie je `OutputFormat`-Konstante (Json,
   MarkdownForAi, Html). `Journal` wird hier standardmäßig **nicht** mit
   ausgegeben (separates Aggregat, separater Query-Pfad) – könnte optional
   als Erweiterung des Exports ergänzt werden.
4. Bei Ablauf/Widerruf: `410 Gone`.

---

## 7. Verteilungssicht

```
.NET Aspire AppHost
├─ TopSpecs.Api          (Container/Prozess)
├─ TopSpecs.Web           (statisch ausgeliefert oder eigener Container)
├─ PostgreSQL               (Container lokal, verwalteter Dienst in Produktion)
├─ Keycloak                  (Container, OIDC-Provider)
└─ ServiceDefaults            (OpenTelemetry, Health Checks – einheitlich)
```

---

## 8. Querschnittliche Konzepte

### 8.1 CQRS + Vertical Slices

MediatR als Vermittler; jedes Slice unter `UseCases/<Kontext>/<Aggregat>/<Feature>/`
enthält Command/Query, Handler, Validator und ggf. Specifications.

### 8.2 Unit of Work & Repository-Trennung

- `IUnitOfWork.SaveChangesAsync()` am Ende jedes Command-Handlers.
- `IWriteRepository<T>`: `AddAsync`, `UpdateAsync`, `DeleteAsync`. `DeleteAsync`
  löscht **nicht** physisch, sondern ruft eine Domain-Methode auf, die
  `AuditInfo.OnDelete(...)` setzt (Soft-Delete, siehe 5.2) – die Zeile bleibt
  über einen globalen Query-Filter aus normalen Abfragen ausgeblendet.
- `IReadRepository<T>`: `GetByIdAsync`, `FindAsync(Specification<T>)`,
  `ListAsync(Specification<T>)`.
- Pro Bounded Context eigene Repository-Interfaces – keine geteilten
  Repositories über die Kontextgrenze hinweg.

### 8.3 Specification Pattern – Namenskonvention

Um Verwechslung mit der Domänen-Entity `Spec` zu vermeiden, werden
Specification-Klassen **immer ausgeschrieben**:

```csharp
public sealed class AssetTreeByIdSpecification : Specification<Asset>
{
    public AssetTreeByIdSpecification(AssetId id)
    {
        Query.Where(a => a.Id == id)
             .Include(a => a.Components).ThenInclude(c => c.Children);
    }
}
```

Diese Regel gehört in `AGENTS.md`/`my-voice.md` als verbindliche Coding-Konvention.

### 8.4 Persistenzstrategie: rekursive Components + JSONB-Specs

- `Component` wird als **eigene, self-referencing Tabelle** (`ParentComponentId`)
  abgebildet.
- `Spec`-Werte liegen als **JSONB-Spalte** – sowohl direkt auf `Asset` als auch
  auf `Component`, da beide `IHasSpecs` implementieren.
- `Journal` ist eine eigene Tabelle (nicht in der JSONB-Spalte des Assets),
  da es unbegrenzt wächst und beim normalen Asset-Laden nicht automatisch
  mitgeladen werden soll.
- `Attachment` speichert nur Metadaten in PostgreSQL; die eigentliche Datei
  liegt in einem separaten Blob-Storage (Docker-Volume lokal, S3-kompatibel
  produktiv), referenziert über `StorageKey` – vermeidet großes Binärgewicht
  in der relationalen Datenbank.
- Trade-off: Cross-Component-Filter auf einzelne Spec-Werte benötigen
  PostgreSQL-JSONB-Operatoren (`->>`, GIN-Index), gekapselt im
  Repository-/Specification-Layer.

### 8.5 Bounded-Context-Isolation

- `Inventory` referenziert aus `Templates` **ausschließlich** die typisierten
  ID-Typen (leichte Kopplung) – keine Aggregate, keine Business-Logik, keine
  Repositories.
- Datenübergabe beim Anlegen aus Vorlage nur über **Snapshot-DTOs**
  (Published Language, siehe 5.9).
- Durchsetzung optional über `NetArchTest`.

### 8.6 DDD-Bausteine (SharedKernel)

`EntityBase`, `IEntityBase`, `AggregateRoot`, `Entity`, `ValueObject`,
`AuditInfo`, `UserId`, `Result<T>`, `Specification<T>`, Guard-Clauses.

### 8.7 Validierung

Hybrid: FluentValidation im Handler nur für Cross-Entity-Prüfungen; alle
Domain-Invarianten (Rekursionstiefe falls künftig begrenzt, zulässige
Ownership-Übergänge, „Journal-Eintrag braucht Notes oder Measurements") leben
in den jeweiligen Aggregaten selbst.

**Standard-Spec vs. Custom-Spec:** `Spec.TemplateId` (nullable) entscheidet,
kein zusätzliches Flag nötig – `IsCustom` ist rein abgeleitet
(`TemplateId is null`). Die Validierungstiefe unterscheidet sich aber bewusst:

- **Standard-Spec** (`TemplateId` gesetzt): Handler lädt vorab per
  Cross-Context-Query einen `SpecTemplateSnapshot` aus dem `Templates`-Kontext
  und prüft `Value` gegen dessen `DataType`/`Unit` – klassische
  Cross-Entity-Validierung (hier sogar Cross-Context), gehört laut Hybrid-Regel
  in den Handler.
- **Custom-Spec** (`TemplateId` = null): keine Cross-Context-Prüfung möglich
  (es gibt keine Vorlage) – nur die Basis-Invariante im `Asset`/`Component`
  selbst (`Key` nicht leer, `Value` nicht leer).

### 8.8 Sicherheit

- Web-UI: **Keycloak** (OIDC).
- ShareLinks: tokenbasiert, zeitlich befristet, kein Login – striktes
  Rate-Limiting, keine sensiblen Zusatzdaten im Export.

### 8.9 Testkonzept

| Ebene | Projekt | Fokus |
|---|---|---|
| Unit | `SharedKernel.UnitTests` | `AuditInfo`-Übergänge (`OnCreate`/`OnUpdate`/`OnDelete`), `IsDeleted`-Ableitung |
| Unit | `Domain.Inventory.UnitTests` | Asset-/Component-Invarianten, Ownership, LifecycleStatus, Rekursion, Journal-/Attachment-/AssetRelationship-Invarianten |
| Unit | `Domain.Templates.UnitTests` | AssetTemplate-/ComponentTemplate-Invarianten |
| Unit | `UseCases.UnitTests` | Slice-Handler mit gemockten Repositories/UoW, inkl. Snapshot-Mapping |
| Integration | `Infrastructure.IntegrationTests` | EF Core + JSONB gegen Testcontainer-Postgres |
| Funktional | `Api.FunctionalTests` | End-to-End über HTTP inkl. Keycloak-Testrealm |
| Architektur | `Architecture.Tests` | NetArchTest: Bounded-Context-Isolation erzwingen |

### 8.10 Internationalisierung

Backend liefert ausschließlich Keys, keine deutschen Texte, keine Icons im
Domänencode. Frontend mapped via i18n und setzt CSS-Klassen auf Basis des
Status-Strings.

---

## 9. Architekturentscheidungen (ADR-Kurzform)

| # | Entscheidung | Begründung | Alternativen (verworfen) |
|---|---|---|---|
| 1 | `Bubble` statt `Space` | Bewusste Namenswahl, konsequent in Code/UI/Doku | `Space` |
| 2 | `Asset` statt `Item` | Konsistent zu `AssetTemplate`, passt zum Ownership-Konzept | `Item`, `Object`, `Unit` (Namenskollisionen) |
| 3 | Zwei Bounded Contexts: `Inventory` und `Templates` | Klare fachliche Trennung: aktuelle Bestände vs. wiederverwendbare Vorlagen | Ein einziger Kontext |
| 4 | Leichte Kopplung über typisierte IDs zwischen den Kontexten | Pragmatischer Mittelweg für Solo-Projekt | Strikte Isolation mit doppelten ID-Typen |
| 5 | `Component`/`ComponentTemplate` rekursiv | Beliebig tiefe Bauteil-Hierarchien ohne zusätzlichen Typ | Fixe Zwei-Ebenen-Struktur |
| 6 | `Ownership` auf Asset- **und** Component-Ebene | Granularer Status pro Bauteil möglich | Ownership nur am Asset |
| 7 | Specification-Pattern-Klassen immer ausgeschrieben | Vermeidet Verwechslung mit Entity `Spec` | Abkürzung `...Spec` |
| 8 | Specs als JSONB-Spalte statt normalisierter Tabelle | Dynamische Specs ohne Schema-Migration | Relationale `Spec`-Tabelle (EAV) |
| 9 | Cross-Context-Datenübergabe nur via Snapshot-DTO | Keine Kopplung an Templates-Domänenlogik | Direkter Zugriff auf Templates-Aggregate |
| 10 | ShareLink nur auf `Asset` (Wurzel) | Klare Aggregatgrenze | ShareLink auf beliebiges Component |
| 11 | `Spec` kann direkt am `Asset` hängen (`IHasSpecs`) | Trivialer Gegenstand braucht keine erzwungene Component-Zerlegung; entspricht CMDB-/AAS-Praxis | Specs nur über Component (unnötige Indirektion bei einfachen Assets) |
| 12 | `AssetIdentity` als ValueObject (Seriennummer/Tag) | Realer Bezug zum physischen Exemplar – Kern eines „digitalen Zwillings" | Keine Identität (Asset bliebe reiner Datensatz ohne Bezug zum echten Objekt) |
| 13 | `Journal` statt separatem `MeasurementLog` | Ein generisches Konzept für Notizen **und** Messwerte statt mehrerer spezialisierter Entities | Getrennte Entities pro Anwendungsfall (Werkstatt, Messwert, Notiz) |
| 14 | `Journal` als eigenständiges Aggregat mit Eintrags-Liste | Unbegrenztes Wachstum würde `Asset` beim Laden aufblähen; eine Liste pro Ziel statt Aggregat pro Eintrag vermeidet Explosion kleinster Aggregate | Aggregat pro einzelnem Eintrag (zu granular) |
| 15 | `Unit` als validierendes ValueObject (offene Wertemenge) | Type-Safety ohne feste Werteliste – anders als `Ownership` | Enum (verboten), roher `string` (kein Schutz vor leeren/inkonsistenten Werten) |
| 16 | `AssetRelationship` aktiviert (nicht mehr ausgeklammert): Asset↔Asset, Component↔Asset, symmetrisch auch Component↔Component | Realer Bedarf bestätigt (PV-Anlage↔Wallbox); symmetrische Struktur (optionale ComponentId auf beiden Seiten) ist einfacher als eine asymmetrische Sonderregel | Ursprünglich ausgeklammert (kein Bedarf erkennbar) – durch Praxis-Beispiel widerlegt; künstliche Beschränkung auf nur zwei der drei Kombinationen (unnötige Komplexität ohne Nutzen) |
| 17 | `IHasSpecs` statt `ISpecCarrier` | Konsistent zum bestehenden Namensmuster `IHasDisplayName` | `ISpecCarrier` (eigenes, sonst ungenutztes Namensmuster) |
| 18 | Kein separates `CustomSpec`/`IsCustom`-Feld, sondern abgeleitete Property aus `TemplateId is null` | Vermeidet redundante Datenhaltung (DRY) | Eigenes `IsCustom`-Feld oder eigene `CustomSpec`-Klasse |
| 19 | Standard-Spec-Werte werden bei nachträglicher Nutzeränderung gegen `SpecTemplate` validiert, Custom-Spec bleibt frei | Konsistenz zur Vorlage bei manueller Änderung, ohne Ad-hoc-Specs künstlich einzuschränken | Keine Validierung für Standard-Specs (Inkonsistenz zur Vorlage möglich) |
| 20 | `LifecycleStatus` als eigenes ValueObject, getrennt von `Ownership` | Unterschiedliche fachliche Bedeutung: Anschaffungsstatus vs. Betriebszustand danach; ein Asset kann `Owned` und gleichzeitig `InRepair` sein | Wiederverwendung von `Ownership` mit zusätzlichen Werten (vermischt zwei Konzepte) |
| 21 | `Attachment` als eigenständiges Aggregat (wie `Journal`) | Unbegrenztes Wachstum, kein automatisches Mitladen beim Asset-Zugriff nötig | Anhänge als Liste im `Asset`-Aggregat |
| 22 | Attachment-Datei in Blob-Storage, nur Metadaten in PostgreSQL | Vermeidet große Binärdaten in der relationalen Datenbank | Datei direkt als `bytea`/JSONB in PostgreSQL |
| 23 | `Location` nur auf Asset-Ebene, nicht auf Component | Ein Standort ist meist nur für den Gesamtgegenstand sinnvoll (wo steht das Rennrad/die PV-Anlage) – auf Component-Ebene i. d. R. redundant | Location auch auf Component-Ebene (unnötige Komplexität ohne erkennbaren Nutzen) |
| 24 | `SpecDataType` als eigenes ValueObject (geschlossene Wertemenge), statt `DataType` als roher `string` | Konsistent zur „keine Enums, aber typsichere ValueObjects"-Regel; `DataType` ist im Gegensatz zu `Unit` eine geschlossene, nicht offene Wertemenge | `DataType` als `string` (Inkonsistenz zur eigenen Namenskonvention) |
| 25 | MCP-Anbindung (geplant): erste Ausbaustufe scoped auf ein einzelnes `Asset`, Auth über bestehenden `ShareLink`-Token statt eigenem API-Key-Konzept | Wiederverwendet vorhandene, bereits befristete/widerrufbare Credential-Infrastruktur; kein zusätzliches Sicherheitsmodell nötig | Separates API-Key-Aggregat (redundant zu `ShareLink`), breiterer Multi-Asset-Scope sofort (mehr Sicherheitsfläche, noch nicht nötig) |
| 26 | `IHasDescription` auf allen Kern-Entities beider Kontexte (`Bubble`, `Asset`, `Component`, `Spec`, `AssetTemplate`, `ComponentTemplate`, `SpecTemplate`) | Konsistentes, dauerhaftes Freitextfeld für Kontext/Zielsetzung – abgegrenzt von `Journal` (datierte Einzeleinträge); plain `string?`, keine ValueObject-Hülle nötig (keine Invariante außer optionaler Länge) | Nur auf einzelnen Ebenen (uneinheitlich, schwer zu merken, wo es existiert) |
| 27 | `Spec.Value` bleibt vorerst skalarer `string`, keine Liste (`Values`) | Pragmatisch vertagt: löst das eigentliche Motiv (mehrpunktige Fan-Kurven) ohnehin nicht sauber, da das strukturierte/gepaarte Daten sind, keine flache Liste gleichartiger Werte | Sofortige Umstellung auf `IReadOnlyList<string>` (Aufwand jetzt, ohne den eigentlichen Anwendungsfall zu lösen) |
| 28 | Fan-Kurven u. ä. strukturierte Werte vorerst als ein einzelner Freitext-`Spec.Value` | Pragmatisch, keine Modelländerung nötig (`Value` ist bereits `string`); bewusst nicht auswertbar | Eigenes strukturiertes Werteformat (JSON-Objekt je Spec) – zurückgestellt, bis echter Auswertungsbedarf entsteht |
| 29 | `SpecHistory` wieder entfernt | Ursprünglich aus abstrakter Digital-Twin-Vollständigkeit eingeführt, ohne konkreten Bedarf; kein Beispiel aus echten Nutzdaten (Auto/PC/Rennrad) zeigte einen Fall dafür – anders als `Journal`, das jeden Zeitverlaufs-Fall abdeckte. Zusätzlich fehlte zu dem Zeitpunkt sogar der einfachere Baustein (`UpdatedAt`) noch, auf dem `SpecHistory` hätte aufbauen müssen – YAGNI-Verstoß | Beibehalten „für später" (Komplexität ohne belegten Nutzen); bei echtem Bedarf zuerst `UpdatedAt` ergänzen, `SpecHistory` erst danach falls nötig |
| 30 | `AuditInfo` (nicht `Meta`) als ValueObject auf `EntityBase<TId>`, gilt für **jedes** Entity beider Kontexte | Gängigerer Name in .NET-Clean-Architecture-Referenzen (z. B. Jason Taylors Template); universell statt Opt-in-Interface, da ausnahmslos jedes Entity betroffen ist | `Meta`/`Metadata` (eher für freie Zusatzinfos üblich, nicht für Audit-Trail); pro Entity wiederholte Felder statt gemeinsamer Basisklasse |
| 31 | Automatische Befüllung von `AuditInfo` über EF-Core-`SaveChangesInterceptor`, nicht manuell in Handlern | Konsistent garantiert, kein Vergessen möglich; Infrastructure-Zuständigkeit, keine Domain-/UseCases-Logik nötig | Manuelles Setzen in jedem Command-Handler (fehleranfällig, Wiederholung) |
| 32 | Optimistic-Concurrency-Token über PostgreSQL-`xmin`, keine eigene Domain-Property | Native Npgsql-Unterstützung, reine Infrastructure-Konfiguration; Nebenläufigkeit ist kein Domänenkonzept | Eigenes `RowVersion`-Property auf `EntityBase` (unnötige Domain-Verunreinigung mit technischem Detail) |
| 33 | Soft-Delete (`DeletedAt`/`DeletedBy`) als dritter Wertepaar in `AuditInfo`, nicht separates Konzept | Fachlich dasselbe Muster wie Created/Updated (Zeitpunkt + Nutzer); ein zusammengehöriges ValueObject statt verstreuter Felder | Separates `SoftDeleteInfo`-ValueObject oder lose Properties direkt auf `EntityBase` |

---

## 10. Qualitätsanforderungen

### 10.1 Qualitätsbaum (Auszug)

```
Qualität
├─ Erweiterbarkeit
│   └─ Neue Asset-Art ohne Codeänderung (nur neues AssetTemplate + SpecTemplates)
├─ Sicherheit
│   └─ Abgelaufener/widerrufener ShareLink liefert keine Daten (410 Gone)
├─ Performance
│   ├─ ShareLink-Auflösung < 300ms bei typischem Asset-Baum (< 50 Specs, < 20 Components)
│   └─ Journal-Zugriff unabhängig von Asset-Ladezeit (separates Aggregat)
└─ Wartbarkeit
    ├─ Domain-Schicht ohne Infrastruktur-Referenzen (Architektur-Test erzwungen)
    └─ Inventory referenziert aus Templates nur ID-Typen (Architektur-Test erzwungen)
```

---

## 11. Risiken und technische Schulden

| Risiko | Auswirkung | Umgang |
|---|---|---|
| Unbegrenzte `Component`-/`ComponentTemplate`-Rekursionstiefe | UI-Darstellung/Performance | Bewusste Tiefenbegrenzung als Domain-Invariante erwägen (z. B. `MaxDepth`), Entscheidung vertagt |
| Leichte Kopplung könnte über Zeit „aufweichen" | Versehentlicher Zugriff auf Templates-Aggregate aus Inventory | Architektur-Test (NetArchTest) von Anfang an einziehen |
| JSONB-Queries auf einzelne Spec-Werte | Komplexere Specifications, GIN-Index-Pflege nötig | Kapselung im Repository-/Specification-Layer |
| `Journal` wächst unbegrenzt | Bei sehr häufigen Einträgen (z. B. täglicher PV-Ertrag über Jahre) potenziell große Aggregate | Paginierte Query statt vollständigem Laden, sobald relevant – keine Änderung am Aggregat nötig |
| Blob-Storage für Attachments noch nicht final entschieden | Lokaler Docker-Volume-Speicher ist für ein Solo-Projekt praktikabel, skaliert aber nicht ohne Weiteres | Abstraktion über ein `IBlobStorage`-Interface von Anfang an, konkrete Implementierung austauschbar |
| Globaler Query-Filter für Soft-Delete vergessen | Gelöschte Zeilen tauchen fälschlich in normalen Abfragen auf | Filter zentral in der `DbContext`-Konfiguration je Entity setzen, per Architektur-Test absichern |
| `UpdatedAt`/`UpdatedBy` und `DeletedAt`/`DeletedBy` können nach einer Löschung identisch aussehen | Interceptor setzt bei `Modified` immer `OnUpdate` – auch wenn die Änderung eigentlich eine Löschung war | Funktional unschädlich (beide Werte korrekt), nur redundant; bei Bedarf im Interceptor gezielt unterscheiden |
| ShareLink ohne Login | Angriffsfläche (Token-Erraten) | Ausreichend lange Tokens, Rate-Limiting, kurze Standard-Gültigkeit |
| Ein Token für Web-Ansicht **und** MCP-Zugriff (geplant) | Geleakter Token gewährt nicht nur Lesezugriff auf eine Seite, sondern eine abfragbare Schnittstelle | Bei Detailplanung erneut bewerten – ggf. getrennte Scopes/Berechtigungen pro Token statt vollständiger Wiederverwendung |
| `AssetRelationship` erlaubt auch Component↔Component | Größere Kombinationsvielfalt als ursprünglich angedacht – potenziell unübersichtliche Beziehungsnetze bei vielen Einträgen | Bewusst in Kauf genommen (ADR #16); bei Bedarf später UI-seitig filtern/visualisieren, keine Domain-Änderung nötig |
| Mehrpunktige/strukturierte Spec-Werte (z. B. Fan-Kurven) nur als Freitext | Nicht auswertbar/nicht abfragbar über JSONB-Operatoren | Bewusst vertagt (ADR #27/#28); bei echtem Bedarf strukturiertes Format nachrüsten |
| Einzelentwickler | Bus-Faktor 1 | Architektur konventionell/dokumentiert halten (arc42, AGENTS.md, my-voice.md) |

---

## 12. Glossar

| Begriff | Bedeutung |
|---|---|
| **Bubble** | Themenbereich/Kategorie, dem Assets zugeordnet werden |
| **Asset** | Konkreter Gegenstand, Aggregate Root im Inventory-Kontext, trägt Specs direkt oder über Components |
| **Component** | Bauteil eines Assets, selbstreferenzierend, trägt ebenfalls Specs |
| **Spec** | Einzelne Spezifikation (Key/Value/Unit), an Asset oder Component, persistiert als JSONB. `IsCustom` (abgeleitet aus `TemplateId is null`) unterscheidet Standard- von Ad-hoc-Specs |
| **IHasSpecs** | Gemeinsame Schnittstelle von `Asset` und `Component` für das Tragen von Specs (Namensmuster analog `IHasDisplayName`) |
| **Ownership** | ValueObject für Anschaffungsstatus (`Owned`, `Planning`, `Wishlist`), auf Asset- und Component-Ebene |
| **LifecycleStatus** | ValueObject für Betriebszustand (`Active`, `InRepair`, `Sold`, `Disposed`), unabhängig von `Ownership`, auf Asset- und Component-Ebene |
| **Location** | ValueObject für den Standort eines Assets (Freitext-Label, optional Koordinaten), nur auf Asset-Ebene |
| **AssetIdentity** | ValueObject für reale Identität (Seriennummer, Asset-Tag) |
| **Unit** | Validierendes ValueObject für Maßeinheiten (offene Wertemenge, kein Enum) |
| **SpecDataType** | ValueObject für den erlaubten Werttyp eines SpecTemplate (`string`, `number`, `boolean`, `date`) – geschlossene Wertemenge, anders als `Unit` |
| **PurchaseLink** | Optionaler Shop-/Affiliate-Link an Asset/Component |
| **Journal** | Aggregat für Freitext-/Messwert-Verlauf zu einem Asset/Component (z. B. Werkstatt-Ergebnisse) |
| **JournalEntry** | Einzelner Eintrag im Journal (Kategorie, Notizen, Messwerte, Zeitpunkt) |
| **Measurement** | ValueObject für einen einzelnen Messwert innerhalb eines JournalEntry |
| **Attachment** | Aggregat für eine Datei-Referenz (Foto, Handbuch, Rechnung) an Asset-, Component- oder Spec-Ebene; Datei selbst liegt im Blob-Storage |
| **ShareLink** | Zeitlich befristeter, formatspezifischer Freigabelink für ein Asset |
| **AssetTemplate** | Wiederverwendbare Vorlage für eine Asset-Art, Aggregate Root im Templates-Kontext |
| **ComponentTemplate** | Vorlage für ein Component, selbstreferenzierend |
| **SpecTemplate** | Vorlage für eine Spec (Datentyp, Einheit, Pflichtfeld) |
| **Bounded Context Inventory** | Fachlicher Kontext für aktuelle Bestände |
| **Bounded Context Templates** | Fachlicher Kontext für wiederverwendbare Vorlagen |
| **AssetRelationship** | Aggregat für fachliche Beziehungen zwischen zwei Assets/Components (z. B. „Wechselrichter versorgt Wallbox"); Asset↔Asset, Component↔Asset und Component↔Component möglich |
| **RelationType** | ValueObject für die Art einer `AssetRelationship` (offene Wertemenge, z. B. „PowersTarget", „ConnectedTo") |
| **IHasDescription** | Gemeinsame Schnittstelle für ein dauerhaftes Freitextfeld, implementiert von `Bubble`, `Asset`, `Component`, `Spec`, `AssetTemplate`, `ComponentTemplate`, `SpecTemplate` |
| **EntityBase** | Gemeinsame Basisklasse in `SharedKernel`, von der `AggregateRoot<TId>` und `Entity<TId>` ableiten; trägt `AuditInfo` automatisch für jedes Entity |
| **AuditInfo** | ValueObject für Erstellung/Änderung/Löschung (je Zeitpunkt + `UserId`), inkl. Soft-Delete (`IsDeleted`, abgeleitet aus `DeletedAt`) |
| **UserId** | ValueObject, wrapt die Keycloak-Subject-ID; kein eigenes User-Aggregat im Domänenmodell |
