using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Infrastructure.Data
{
    /// <summary>
    /// Seeds the database with initial data including Egyptian Museum sites, 84 artifacts, and admin account
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly RuyaContext _context;
        private readonly UserManager<User> _userManager;

        public DatabaseSeeder(RuyaContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Seeds all initial data
        /// </summary>
        public async Task SeedAllAsync()
        {
            await SeedAdminUserAsync();
            await SeedSitesAsync();
            await SeedArtifactsAsync();
        }

        /// <summary>
        /// Seeds admin user account
        /// </summary>
        private async Task SeedAdminUserAsync()
        {
            const string adminEmail = "admin@ruya.com";
            const string adminPassword = "Admin@123";

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            
            if (existingAdmin != null)
            {
                Console.WriteLine("⏭️  Admin user already exists. Skipping admin seeding.");
                return;
            }

            Console.WriteLine("👤 Creating admin user...");

            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                PhoneNumber = "+1234567890",
                FullName = "System Administrator",
                PreferredLanguage = "en",
                KnowledgeLevel = "Expert"
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"✅ Admin user created successfully.");
                Console.WriteLine($"   Email: {adminEmail}");
                Console.WriteLine($"   Password: {adminPassword}");
            }
            else
            {
                Console.WriteLine("❌ Failed to create admin user:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"   - {error.Description}");
                }
            }
        }

        /// <summary>
        /// Seeds museum sites with bilingual translations
        /// </summary>
        private async Task SeedSitesAsync()
        {
            if (await _context.Sites.AnyAsync())
            {
                Console.WriteLine("⏭️  Sites already exist. Skipping site seeding.");
                return;
            }

            var sites = new List<Site>
            {
                new Site
                {
                    Latitude = 30.0478f,
                    Longitude = 31.2336f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Egyptian Museum (Cairo)",
                            City = "Cairo",
                            Country = "Egypt",
                            Hours = "9:00 AM - 5:00 PM",
                            Ticket = "200 EGP",
                            Crowds = "High",
                            Description = "The Museum of Egyptian Antiquities, known commonly as the Egyptian Museum, in Cairo, Egypt, is home to an extensive collection of ancient Egyptian antiquities."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "المتحف المصري (القاهرة)",
                            City = "القاهرة",
                            Country = "مصر",
                            Hours = "9:00 صباحاً - 5:00 مساءً",
                            Ticket = "200 جنيه مصري",
                            Crowds = "مرتفع",
                            Description = "متحف الآثار المصرية، المعروف عموماً بالمتحف المصري، في القاهرة، مصر، يضم مجموعة واسعة من الآثار المصرية القديمة."
                        }
                    }
                },
                new Site
                {
                    Latitude = 29.9932f,
                    Longitude = 31.1173f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Grand Egyptian Museum (GEM)",
                            City = "Giza",
                            Country = "Egypt",
                            Hours = "9:00 AM - 5:00 PM",
                            Ticket = "400 EGP",
                            Crowds = "Very High",
                            Description = "The Grand Egyptian Museum (GEM), also known as the Giza Museum, is an archaeological museum under construction in Giza, Egypt."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "المتحف المصري الكبير",
                            City = "الجيزة",
                            Country = "مصر",
                            Hours = "9:00 صباحاً - 5:00 مساءً",
                            Ticket = "400 جنيه مصري",
                            Crowds = "مرتفع جداً",
                            Description = "المتحف المصري الكبير، المعروف أيضاً بمتحف الجيزة، هو متحف أثري قيد الإنشاء في الجيزة، مصر."
                        }
                    }
                },
                new Site
                {
                    Latitude = 25.6995f,
                    Longitude = 32.6392f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Luxor Temple",
                            City = "Luxor",
                            Country = "Egypt",
                            Hours = "6:00 AM - 9:00 PM",
                            Ticket = "160 EGP",
                            Crowds = "Medium",
                            Description = "Luxor Temple is a large Ancient Egyptian temple complex located on the east bank of the Nile River."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "معبد الأقصر",
                            City = "الأقصر",
                            Country = "مصر",
                            Hours = "6:00 صباحاً - 9:00 مساءً",
                            Ticket = "160 جنيه مصري",
                            Crowds = "متوسط",
                            Description = "معبد الأقصر هو مجمع معابد مصري قديم كبير يقع على الضفة الشرقية لنهر النيل."
                        }
                    }
                },
                new Site
                {
                    Latitude = 29.9792f,
                    Longitude = 31.1342f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Giza Necropolis",
                            City = "Giza",
                            Country = "Egypt",
                            Hours = "8:00 AM - 4:00 PM",
                            Ticket = "200 EGP",
                            Crowds = "Very High",
                            Description = "The Giza pyramid complex is an archaeological site on the Giza Plateau, on the outskirts of Cairo, Egypt."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "جبانة الجيزة",
                            City = "الجيزة",
                            Country = "مصر",
                            Hours = "8:00 صباحاً - 4:00 مساءً",
                            Ticket = "200 جنيه مصري",
                            Crowds = "مرتفع جداً",
                            Description = "مجمع أهرامات الجيزة هو موقع أثري على هضبة الجيزة، على أطراف القاهرة، مصر."
                        }
                    }
                },
                new Site
                {
                    Latitude = 29.8714f,
                    Longitude = 31.2169f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Saqqara",
                            City = "Saqqara",
                            Country = "Egypt",
                            Hours = "8:00 AM - 4:00 PM",
                            Ticket = "150 EGP",
                            Crowds = "Low",
                            Description = "Saqqara, also spelled Sakkara or Saccara, is an Egyptian village in Giza Governorate, that contains ancient burial grounds of Egyptian royalty."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "سقارة",
                            City = "سقارة",
                            Country = "مصر",
                            Hours = "8:00 صباحاً - 4:00 مساءً",
                            Ticket = "150 جنيه مصري",
                            Crowds = "منخفض",
                            Description = "سقارة هي قرية مصرية في محافظة الجيزة، تحتوي على مقابر أثرية قديمة للعائلة المالكة المصرية."
                        }
                    }
                },
                new Site
                {
                    Latitude = 29.7908f,
                    Longitude = 31.2292f,
                    Translations = new List<SiteTranslation>
                    {
                        new SiteTranslation
                        {
                            LanguageCode = "en",
                            Name = "Dahshur",
                            City = "Dahshur",
                            Country = "Egypt",
                            Hours = "8:00 AM - 4:00 PM",
                            Ticket = "100 EGP",
                            Crowds = "Low",
                            Description = "Dahshur is a royal necropolis located in the desert on the west bank of the Nile."
                        },
                        new SiteTranslation
                        {
                            LanguageCode = "ar",
                            Name = "دهشور",
                            City = "دهشور",
                            Country = "مصر",
                            Hours = "8:00 صباحاً - 4:00 مساءً",
                            Ticket = "100 جنيه مصري",
                            Crowds = "منخفض",
                            Description = "دهشور هي جبانة ملكية تقع في الصحراء على الضفة الغربية لنهر النيل."
                        }
                    }
                }
            };

            await _context.Sites.AddRangeAsync(sites);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {sites.Count} sites successfully.");
        }

        /// <summary>
        /// Seeds all 84 Egyptian artifacts that match the Python AI YOLO model with bilingual translations
        /// </summary>
        private async Task SeedArtifactsAsync()
        {
            if (await _context.Artifacts.AnyAsync())
            {
                Console.WriteLine("⏭️  Artifacts already exist. Skipping artifact seeding.");
                return;
            }

            // Get site IDs for assignment
            var egyptianMuseum = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Egyptian Museum (Cairo)")));
            var gem = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Grand Egyptian Museum")));
            var luxorTemple = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Luxor")));
            var giza = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Giza Necropolis")));
            var saqqara = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Saqqara")));
            var dahshur = await _context.Sites.Include(s => s.Translations).FirstOrDefaultAsync(s => s.Translations.Any(t => t.Name.Contains("Dahshur")));

            int defaultSiteId = egyptianMuseum?.Id ?? 1;
            int gemSiteId = gem?.Id ?? defaultSiteId;
            int luxorSiteId = luxorTemple?.Id ?? defaultSiteId;
            int gizaSiteId = giza?.Id ?? defaultSiteId;
            int saqqaraSiteId = saqqara?.Id ?? defaultSiteId;
            int dahshurSiteId = dahshur?.Id ?? defaultSiteId;

            // All 84 artifacts matching Python YOLO model ARTIFACT_MAPPING with bilingual translations
            var artifacts = new List<Artifact>
            {
                // Class 0-9
                CreateArtifact(defaultSiteId, "Akhenaten", "أخناتون", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Sandstone, Limestone", "حجر رملي، حجر جيري", "Karnak Temple, Amarna", "معبد الكرنك، أمارنا"),
                CreateArtifact(defaultSiteId, "Amenhotep III", "أمنحتب الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite, Granite", "كوارتزيت، جرانيت", "Thebes (Luxor)", "طيبة (الأقصر)"),
                CreateArtifact(defaultSiteId, "Amenhotep III and Tiye", "أمنحتب الثالث وتي", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone", "حجر جيري", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Amenhotep III with Plate", "أمنحتب الثالث مع اللوحة", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Stone", "حجر", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Augustus", "أغسطس", "Statue", "تمثال", "Greco-Roman Egypt", "مصر اليونانية الرومانية", "Ptolemaic Period", "العصر البطلمي", "Marble", "رخام", "Alexandria", "الإسكندرية"),
                CreateArtifact(dahshurSiteId, "Bent Pyramid of King Sneferu", "الهرم المنحني للملك سنفرو", "Pyramid", "هرم", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Dahshur", "دهشور"),
                CreateArtifact(defaultSiteId, "Black Granite Bust of Mentuemhat", "تمثال نصفي من الجرانيت الأسود لمنتوإمحات", "Bust", "تمثال نصفي", "Ancient Egypt", "مصر القديمة", "Late Period", "العصر المتأخر", "Black Granite", "جرانيت أسود", "Karnak Temple, Thebes", "معبد الكرنك، طيبة"),
                CreateArtifact(defaultSiteId, "Bust of Isis", "تمثال نصفي لإيزيس", "Bust", "تمثال نصفي", "Ancient Egypt", "مصر القديمة", "Ptolemaic Period", "العصر البطلمي", "Limestone", "حجر جيري", "Various temples", "معابد مختلفة"),
                CreateArtifact(defaultSiteId, "Clossal Head of the god Serapis", "رأس ضخم للإله سيرابيس", "Head", "رأس", "Greco-Roman Egypt", "مصر اليونانية الرومانية", "Ptolemaic Period", "العصر البطلمي", "Marble", "رخام", "Alexandria", "الإسكندرية"),
                CreateArtifact(defaultSiteId, "Clossal head of Senwosret 1", "رأس ضخم لسنوسرت الأول", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Karnak, Lisht", "الكرنك، اللشت"),

                // Class 10-19
                CreateArtifact(defaultSiteId, "Coffin of Ahmose I", "تابوت أحمس الأول", "Coffin", "تابوت", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Wood, Gold leaf", "خشب، ورق ذهب", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Amenhotep III", "تمثال ضخم لأمنحتب الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite, Granite", "كوارتزيت، جرانيت", "Thebes (Luxor)", "طيبة (الأقصر)"),
                CreateArtifact(defaultSiteId, "Colossal Statue of God Ptah", "تمثال ضخم للإله بتاح", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis, Karnak", "ممفيس، الكرنك"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Hormoheb", "تمثال ضخم لحورمحب", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Karnak Temple", "معبد الكرنك"),
                CreateArtifact(defaultSiteId, "Colossal Statue of King Senwosret IlI", "تمثال ضخم للملك سنوسرت الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Middle Kingdom King", "تمثال ضخم لملك الدولة الوسطى", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Various sites", "مواقع مختلفة"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Queen Hatshepsut", "تمثال ضخم للملكة حتشبسوت", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone", "حجر جيري", "Deir el-Bahari, Thebes", "دير البحري، طيبة"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Ramesses II", "تمثال ضخم لرمسيس الثاني", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis, Tanis", "ممفيس، تانيس"),
                CreateArtifact(defaultSiteId, "Colossal Statue of Ramesses II beloved of Ptah", "تمثال ضخم لرمسيس الثاني محبوب بتاح", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),
                CreateArtifact(luxorSiteId, "Colossoi of Memnon", "تمثالا ممنون", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite", "كوارتزيت", "West Bank, Thebes", "البر الغربي، طيبة"),

                // Class 20-29
                CreateArtifact(defaultSiteId, "Colossus of Senuseret I", "تمثال ضخم لسنوسرت الأول", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Column of Merenptah", "عمود مرنبتاح", "Architectural Element", "عنصر معماري", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Granite Statue of Osiris", "تمثال جرانيت لأوزيريس", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Abydos", "أبيدوس"),
                CreateArtifact(defaultSiteId, "Granite Statue of Tutankhamun", "تمثال جرانيت لتوت عنخ آمون", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite, Quartzite", "جرانيت، كوارتزيت", "Valley of the Kings (KV62), Luxor", "وادي الملوك (KV62)، الأقصر"),
                CreateArtifact(gizaSiteId, "Great Pyramids of Giza", "أهرامات الجيزة العظيمة", "Pyramid", "هرم", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone, Granite", "حجر جيري، جرانيت", "Giza Plateau", "هضبة الجيزة"),
                CreateArtifact(defaultSiteId, "Grey Granite of Ramesses II", "جرانيت رمادي لرمسيس الثاني", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Grey Granite", "جرانيت رمادي", "Tanis", "تانيس"),
                CreateArtifact(defaultSiteId, "Hathor Capital", "تاج حتحور", "Architectural Element", "عنصر معماري", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Sandstone", "حجر رملي", "Dendera Temple", "معبد دندرة"),
                CreateArtifact(defaultSiteId, "Hatshepsut face", "وجه حتشبسوت", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone", "حجر جيري", "Deir el-Bahari", "دير البحري"),
                CreateArtifact(defaultSiteId, "Head Statue of Amenhotep III", "تمثال رأس أمنحتب الثالث", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite", "كوارتزيت", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Head Statue of Amenhotep iii", "تمثال رأس أمنحتب الثالث", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite", "كوارتزيت", "Thebes", "طيبة"),

                // Class 30-39
                CreateArtifact(saqqaraSiteId, "Head of Userkaf", "رأس أوسركاف", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Schist", "شست", "Saqqara", "سقارة"),
                CreateArtifact(defaultSiteId, "Hor I", "حور الأول", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Wood, Gold", "خشب، ذهب", "Dahshur", "دهشور"),
                CreateArtifact(defaultSiteId, "Isis with her child", "إيزيس مع طفلها", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Late Period", "العصر المتأخر", "Bronze", "برونز", "Various temples", "معابد مختلفة"),
                CreateArtifact(defaultSiteId, "King Amenemhat 3", "الملك أمنمحات الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Fayum", "الفيوم"),
                CreateArtifact(defaultSiteId, "King Thutmose III", "الملك تحتمس الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Schist", "شست", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Mask of Thuya", "قناع ثويا", "Funerary Mask", "قناع جنائزي", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Gilded Cartonnage, Linen", "كارتوناج مذهب، كتان", "Valley of the Kings (KV46), Luxor", "وادي الملوك (KV46)، الأقصر"),
                CreateArtifact(gemSiteId, "Mask of Tutankhamun", "قناع توت عنخ آمون", "Funerary Mask", "قناع جنائزي", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Gold, Lapis Lazuli, Carnelian, Obsidian, Turquoise", "ذهب، لازورد، عقيق، سبج، فيروز", "Valley of the Kings (KV62), Luxor", "وادي الملوك (KV62)، الأقصر"),
                CreateArtifact(defaultSiteId, "Mask of Yuya", "قناع يويا", "Funerary Mask", "قناع جنائزي", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Gilded Cartonnage, Linen", "كارتوناج مذهب، كتان", "Valley of the Kings (KV46), Luxor", "وادي الملوك (KV46)، الأقصر"),
                CreateArtifact(defaultSiteId, "Menkaure Statue", "تمثال منقرع", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Schist (Greywacke)", "شست (جريواكي)", "Giza (Valley Temple)", "الجيزة (معبد الوادي)"),
                CreateArtifact(defaultSiteId, "Mentuhotep Nebhetpre", "منتوحتب نبحتبرع", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Sandstone", "حجر رملي", "Deir el-Bahari", "دير البحري"),

                // Class 40-49
                CreateArtifact(defaultSiteId, "Naos of Senwosert I", "مقصورة سنوسرت الأول", "Shrine", "مقصورة", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "White Limestone", "حجر جيري أبيض", "Karnak Temple", "معبد الكرنك"),
                CreateArtifact(defaultSiteId, "Nefertiti", "نفرتيتي", "Bust", "تمثال نصفي", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone, Stucco", "حجر جيري، جص", "Amarna (Tell el-Amarna)", "أمارنا (تل العمارنة)"),
                CreateArtifact(defaultSiteId, "Obelsik Tip of Hatshepsut", "قمة مسلة حتشبسوت", "Obelisk", "مسلة", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Red Granite", "جرانيت أحمر", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Offering table of Amenemhat 6", "مائدة قرابين أمنمحات السادس", "Relief", "نقش بارز", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Limestone", "حجر جيري", "Lisht", "اللشت"),
                CreateArtifact(saqqaraSiteId, "Pyramid of Djoser", "هرم زوسر", "Pyramid", "هرم", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Saqqara", "سقارة"),
                CreateArtifact(defaultSiteId, "Rhetorical Stela of King Ramesses ll", "لوحة بلاغية للملك رمسيس الثاني", "Stela", "لوحة", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone, Granite", "حجر جيري، جرانيت", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Seated Statue of Amenhotep III", "تمثال جالس لأمنحتب الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Quartzite", "كوارتزيت", "Thebes", "طيبة"),
                CreateArtifact(saqqaraSiteId, "Seated Statue of Djoser", "تمثال جالس لزوسر", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Diorite", "ديوريت", "Saqqara", "سقارة"),
                CreateArtifact(defaultSiteId, "Seated Statue of God Sekhmet", "تمثال جالس للإلهة سخمت", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Karnak, Memphis", "الكرنك، ممفيس"),
                CreateArtifact(defaultSiteId, "Seated Statue of Ramesses II", "تمثال جالس لرمسيس الثاني", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),

                // Class 50-59
                CreateArtifact(defaultSiteId, "Seated Statue of Ramesses II and God Ptah", "تمثال جالس لرمسيس الثاني والإله بتاح", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),
                CreateArtifact(defaultSiteId, "Seated Statue of Thutmose III", "تمثال جالس لتحتمس الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Schist", "شست", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Senwosret III", "سنوسرت الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Deir el-Bahari", "دير البحري"),
                CreateArtifact(gizaSiteId, "Sphinx", "أبو الهول", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Giza Plateau", "هضبة الجيزة"),
                CreateArtifact(defaultSiteId, "Sphinx of Amenmhat III", "أبو الهول لأمنمحات الثالث", "Sphinx", "أبو الهول", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Fayum", "الفيوم"),
                CreateArtifact(defaultSiteId, "Sphinx of Kings Ramesses ll - Merenptah", "أبو الهول للملوك رمسيس الثاني - مرنبتاح", "Sphinx", "أبو الهول", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),
                CreateArtifact(defaultSiteId, "Standing Statue of King Ramses II", "تمثال واقف للملك رمسيس الثاني", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis, Luxor", "ممفيس، الأقصر"),
                CreateArtifact(defaultSiteId, "Standing Statue of Thutmose III", "تمثال واقف لتحتمس الثالث", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Schist", "شست", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Statue Head of Akhenaten", "رأس تمثال أخناتون", "Head", "رأس", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Sandstone", "حجر رملي", "Amarna", "أمارنا"),
                CreateArtifact(defaultSiteId, "Statue of Amenhotep III and God Re-Horakhty", "تمثال أمنحتب الثالث والإله رع حور أختي", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Thebes", "طيبة"),

                // Class 60-69
                CreateArtifact(defaultSiteId, "Statue of Amenmhat I", "تمثال أمنمحات الأول", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Limestone", "حجر جيري", "Lisht", "اللشت"),
                CreateArtifact(defaultSiteId, "Statue of Amun and King", "تمثال آمون والملك", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(defaultSiteId, "Statue of Ankhesenamun", "تمثال عنخ إسن آمون", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Wood, Gold", "خشب، ذهب", "Valley of the Kings", "وادي الملوك"),
                CreateArtifact(defaultSiteId, "Statue of Carcala", "تمثال كاراكالا", "Statue", "تمثال", "Greco-Roman Egypt", "مصر اليونانية الرومانية", "Roman Period", "العصر الروماني", "Marble", "رخام", "Alexandria", "الإسكندرية"),
                CreateArtifact(defaultSiteId, "Statue of God Ptah Ramesses ll Goddess Sekhmet", "تمثال الإله بتاح رمسيس الثاني الإلهة سخمت", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),
                CreateArtifact(defaultSiteId, "Statue of God Ra-Horakhty", "تمثال الإله رع حور أختي", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Heliopolis", "هليوبوليس"),
                CreateArtifact(defaultSiteId, "Statue of Khafre", "تمثال خفرع", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Diorite", "ديوريت", "Giza (Valley Temple)", "الجيزة (معبد الوادي)"),
                CreateArtifact(defaultSiteId, "Statue of Khufu", "تمثال خوفو", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Ivory", "عاج", "Abydos", "أبيدوس"),
                CreateArtifact(defaultSiteId, "Statue of King Ramesses ll - Goddess Anath", "تمثال الملك رمسيس الثاني - الإلهة عنات", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Tanis", "تانيس"),
                CreateArtifact(gemSiteId, "Statue of King Ramses II Grand Egyptian Museum", "تمثال الملك رمسيس الثاني المتحف المصري الكبير", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Mit Rahina (Memphis)", "ميت رهينة (ممفيس)"),

                // Class 70-83
                CreateArtifact(luxorSiteId, "Statue of King Ramses II Luxor Temple", "تمثال الملك رمسيس الثاني معبد الأقصر", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Luxor Temple", "معبد الأقصر"),
                CreateArtifact(defaultSiteId, "Statue of King Sety Il Holding Standards", "تمثال الملك ستي الثاني يحمل الرايات", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(saqqaraSiteId, "Statue of King Zoser", "تمثال الملك زوسر", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Saqqara", "سقارة"),
                CreateArtifact(defaultSiteId, "Statue of Mentuhotep II", "تمثال منتوحتب الثاني", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Sandstone", "حجر رملي", "Deir el-Bahari", "دير البحري"),
                CreateArtifact(defaultSiteId, "Statue of Merenptah as standard Bearer", "تمثال مرنبتاح حامل الراية", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Statue of Osiris", "تمثال أوزيريس", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Stone", "حجر", "Abydos", "أبيدوس"),
                CreateArtifact(defaultSiteId, "Statue of Queen Metnoforet", "تمثال الملكة متنوفرت", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Limestone", "حجر جيري", "Thebes", "طيبة"),
                CreateArtifact(defaultSiteId, "Statue of Ramesses III as standard Bearer", "تمثال رمسيس الثالث حامل الراية", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(dahshurSiteId, "Statue of Snefru", "تمثال سنفرو", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Dahshur", "دهشور"),
                CreateArtifact(defaultSiteId, "Statue of Sobekhotep V", "تمثال سوبك حتب الخامس", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "Middle Kingdom", "الدولة الوسطى", "Granite", "جرانيت", "Karnak", "الكرنك"),
                CreateArtifact(gemSiteId, "Statue of Tutankhamun", "تمثال توت عنخ آمون", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Wood, Gold leaf, Stone", "خشب، ورق ذهب، حجر", "Valley of the Kings (KV62), Luxor", "وادي الملوك (KV62)، الأقصر"),
                CreateArtifact(dahshurSiteId, "Stela of king Snefero", "لوحة الملك سنفرو", "Stela", "لوحة", "Ancient Egypt", "مصر القديمة", "Old Kingdom", "الدولة القديمة", "Limestone", "حجر جيري", "Dahshur", "دهشور"),
                CreateArtifact(defaultSiteId, "bust of Ramesses II", "تمثال نصفي لرمسيس الثاني", "Bust", "تمثال نصفي", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Memphis", "ممفيس"),
                CreateArtifact(defaultSiteId, "kneeling statue of queen hatshibsut", "تمثال راكع للملكة حتشبسوت", "Statue", "تمثال", "Ancient Egypt", "مصر القديمة", "New Kingdom", "الدولة الحديثة", "Granite", "جرانيت", "Deir el-Bahari", "دير البحري")
            };

            await _context.Artifacts.AddRangeAsync(artifacts);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {artifacts.Count} artifacts successfully.");
        }

        private static Artifact CreateArtifact(
            int siteId,
            string nameEn, string nameAr,
            string categoryEn, string categoryAr,
            string civilizationEn, string civilizationAr,
            string periodEn, string periodAr,
            string materialEn, string materialAr,
            string placeEn, string placeAr)
        {
            return new Artifact
            {
                SiteId = siteId,
                ImageUrl = "",
                ImagePublicId = "",
                Translations = new List<ArtifactTranslation>
                {
                    new ArtifactTranslation
                    {
                        LanguageCode = "en",
                        Name = nameEn,
                        Category = categoryEn,
                        Civilization = civilizationEn,
                        Period = periodEn,
                        Material = materialEn,
                        PlaceOfDiscovery = placeEn
                    },
                    new ArtifactTranslation
                    {
                        LanguageCode = "ar",
                        Name = nameAr,
                        Category = categoryAr,
                        Civilization = civilizationAr,
                        Period = periodAr,
                        Material = materialAr,
                        PlaceOfDiscovery = placeAr
                    }
                }
            };
        }
    }
}
