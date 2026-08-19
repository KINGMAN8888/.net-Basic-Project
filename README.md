# ShopApp — مشروع ASP.NET Core MVC للبداية

مشروع بسيط ومكتمل يغطي كل الأساسيات: موديلين بعلاقة بينهم، DbContext، Controllers، Views، Dependency Injection، appsettings، و Migrations.

مبني على **.NET 8** + **EF Core 8** + **SQLite** + **Bootstrap 5 (RTL)**.

---

## 1. التشغيل

```bash
cd ShopApp
dotnet restore
dotnet run
```

افتح: `https://localhost:7xxx` أو `http://localhost:5xxx` (الرابط بيظهر في الترمينال).

> الداتابيز بتتعمل أوتوماتيك أول مرة لأن `Program.cs` بينادي `db.Database.Migrate()`.
> لو عايز تعملها يدوي:
> ```bash
> dotnet tool install --global dotnet-ef
> dotnet ef database update
> ```

---

## 2. هيكل المشروع

```
ShopApp/
├── Models/
│   ├── Category.cs          ← الموديل الأول
│   └── Product.cs           ← الموديل الثاني
├── Data/
│   └── AppDbContext.cs      ← الـ DbContext + العلاقة + Seed Data
├── Services/
│   ├── IProductService.cs   ← الـ Interface (للـ DI)
│   └── ProductService.cs    ← التنفيذ
├── Controllers/
│   ├── CategoriesController.cs  ← بيستخدم DbContext مباشرة
│   ├── ProductsController.cs    ← بيستخدم IProductService (DI)
│   └── HomeController.cs
├── Views/
│   ├── Categories/  (Index, Create, Edit, Details, Delete)
│   ├── Products/    (Index, Create, Edit, Details, Delete)
│   └── Shared/_Layout.cshtml
├── Migrations/              ← ملفات الـ Migration
├── Program.cs               ← نقطة البداية + DI + Middleware + Routing
├── appsettings.json         ← الـ Connection String
└── ShopApp.csproj
```

---

## 3. الموديلات والعلاقة (Relations)

علاقة **One-to-Many**: التصنيف الواحد له عدة منتجات.

```csharp
// Category.cs  ← الطرف "One"
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }  // Navigation Property
}

// Product.cs  ← الطرف "Many"
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }     // Foreign Key
    public Category? Category { get; set; } // Navigation Property
}
```

**قاعدتان مهمتان في EF Core:**

| القاعدة | الشرح |
|---|---|
| `Id` أو `<ClassName>Id` | يتحول تلقائياً إلى Primary Key |
| `<NavigationProperty>Id` | يتحول تلقائياً إلى Foreign Key (`CategoryId` ← `Category`) |

وتم تأكيد العلاقة صراحةً بالـ Fluent API داخل `OnModelCreating`:

```csharp
modelBuilder.Entity<Product>()
    .HasOne(p => p.Category)
    .WithMany(c => c.Products)
    .HasForeignKey(p => p.CategoryId)
    .OnDelete(DeleteBehavior.Cascade);
```

---

## 4. الـ DbContext

`Data/AppDbContext.cs` هو الجسر بين الكود وقاعدة البيانات. كل `DbSet<T>` بيتحول لجدول:

```csharp
public DbSet<Category> Categories => Set<Category>();
public DbSet<Product> Products => Set<Product>();
```

وبيحتوي كمان على **Seed Data** — بيانات مبدئية بتتزرع مع الـ Migration.

---

## 5. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=shopapp.db"
  }
}
```

**للتحويل لـ SQL Server:**

1. غيّر الـ connection string:
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ShopAppDb;Trusted_Connection=True;TrustServerCertificate=True"
   ```
2. ثبّت الباكدج: `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
3. في `Program.cs` بدّل `UseSqlite` بـ `UseSqlServer`
4. امسح فولدر `Migrations` واعمل migration جديدة

---

## 6. Dependency Injection

كل الخدمات بتتسجل في `Program.cs` قبل `builder.Build()`:

```csharp
// تسجيل الـ DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// تسجيل خدمة مخصصة
builder.Services.AddScoped<IProductService, ProductService>();
```

وبعدين بتوصل للكنترولر عن طريق الـ **Constructor**:

```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;   // ASP.NET بيبعتها لوحده
    }
}
```

### أنواع التسجيل الثلاثة

| النوع | العمر | الاستخدام |
|---|---|---|
| `AddSingleton` | نسخة واحدة طول عمر التطبيق | Cache، Configuration |
| `AddScoped` | نسخة لكل HTTP Request | DbContext، Repositories |
| `AddTransient` | نسخة جديدة في كل مرة | خدمات خفيفة بلا حالة |

---

## 7. Program.cs — الترتيب مهم

```
1) builder.Services.Add...()   ← تسجيل الخدمات (DI)
2) var app = builder.Build();
3) app.Use...()                ← الـ Middleware Pipeline (بالترتيب!)
4) app.MapControllerRoute()    ← الـ Routing
5) app.Run();
```

الـ Route الافتراضي:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");
```

يعني `/Products/Edit/3` → `ProductsController.Edit(3)`

---

## 8. Migrations

```bash
# إنشاء migration جديدة بعد أي تعديل في الموديلات
dotnet ef migrations add AddProductDescription

# تطبيقها على الداتابيز
dotnet ef database update

# التراجع عن آخر migration (قبل تطبيقها)
dotnet ef migrations remove

# الرجوع لـ migration معينة
dotnet ef database update InitialCreate

# عرض كل الـ migrations
dotnet ef migrations list
```

> لو `dotnet ef` مش شغال: `dotnet tool install --global dotnet-ef`

---

## 9. الـ Controllers — نمطان للمقارنة

| الكنترولر | الأسلوب | الفايدة |
|---|---|---|
| `CategoriesController` | يستخدم `AppDbContext` مباشرة | أسرع وأبسط للمشاريع الصغيرة |
| `ProductsController` | يستخدم `IProductService` | فصل المسؤوليات + سهولة الـ Unit Testing |

كل كنترولر فيه CRUD كامل: `Index` / `Details` / `Create` / `Edit` / `Delete`.

**ملاحظات أمان مطبقة في الكود:**

- `[ValidateAntiForgeryToken]` على كل POST — حماية من CSRF
- `[Bind("...")]` — حماية من Over-posting (منع المستخدم من التلاعب بحقول ما ينفعش يعدّلها)
- `ModelState.IsValid` — التحقق من صحة البيانات قبل الحفظ

---

## 10. الـ Views

كل View بتستقبل موديل محدد في أول سطر: `@model Product`

أهم الـ Tag Helpers المستخدمة:

| Tag Helper | الوظيفة |
|---|---|
| `asp-for` | ربط الحقل بخاصية في الموديل |
| `asp-action` / `asp-controller` | توليد الروابط |
| `asp-items` | تعبئة الـ dropdown |
| `asp-validation-for` | عرض رسالة الخطأ الخاصة بالحقل |

---

## 11. خطوات التوسعة المقترحة

1. أضف **ViewModels** بدل استخدام الـ Entities مباشرة في الـ Views
2. أضف **Pagination** لصفحة المنتجات
3. أضف **Search & Filter**
4. طبّق **Repository Pattern** بشكل كامل على التصنيفات كمان
5. أضف **ASP.NET Core Identity** لتسجيل الدخول والصلاحيات
6. حوّل المشروع لـ **Web API** بإضافة `[ApiController]` وكنترولرات تحت `/api`
