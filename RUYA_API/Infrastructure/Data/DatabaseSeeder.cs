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
        /// Seeds museum sites
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
                    Name = "Egyptian Museum (Cairo)",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 30.0478f,
                    Longitude = 31.2336f,
                    Hours = "9:00 AM - 5:00 PM",
                    Ticket = "200 EGP",
                    Crowds = "High",
                    Description = "The Museum of Egyptian Antiquities, known commonly as the Egyptian Museum, in Cairo, Egypt, is home to an extensive collection of ancient Egyptian antiquities."
                },
                new Site
                {
                    Name = "Grand Egyptian Museum (GEM)",
                    City = "Giza",
                    Country = "Egypt",
                    Latitude = 29.9932f,
                    Longitude = 31.1173f,
                    Hours = "9:00 AM - 5:00 PM",
                    Ticket = "400 EGP",
                    Crowds = "Very High",
                    Description = "The Grand Egyptian Museum (GEM), also known as the Giza Museum, is an archaeological museum under construction in Giza, Egypt."
                },
                new Site
                {
                    Name = "Luxor Temple",
                    City = "Luxor",
                    Country = "Egypt",
                    Latitude = 25.6995f,
                    Longitude = 32.6392f,
                    Hours = "6:00 AM - 9:00 PM",
                    Ticket = "160 EGP",
                    Crowds = "Medium",
                    Description = "Luxor Temple is a large Ancient Egyptian temple complex located on the east bank of the Nile River."
                },
                new Site
                {
                    Name = "Giza Necropolis",
                    City = "Giza",
                    Country = "Egypt",
                    Latitude = 29.9792f,
                    Longitude = 31.1342f,
                    Hours = "8:00 AM - 4:00 PM",
                    Ticket = "200 EGP",
                    Crowds = "Very High",
                    Description = "The Giza pyramid complex is an archaeological site on the Giza Plateau, on the outskirts of Cairo, Egypt."
                },
                new Site
                {
                    Name = "Saqqara",
                    City = "Saqqara",
                    Country = "Egypt",
                    Latitude = 29.8714f,
                    Longitude = 31.2169f,
                    Hours = "8:00 AM - 4:00 PM",
                    Ticket = "150 EGP",
                    Crowds = "Low",
                    Description = "Saqqara, also spelled Sakkara or Saccara, is an Egyptian village in Giza Governorate, that contains ancient burial grounds of Egyptian royalty."
                },
                new Site
                {
                    Name = "Dahshur",
                    City = "Dahshur",
                    Country = "Egypt",
                    Latitude = 29.7908f,
                    Longitude = 31.2292f,
                    Hours = "8:00 AM - 4:00 PM",
                    Ticket = "100 EGP",
                    Crowds = "Low",
                    Description = "Dahshur is a royal necropolis located in the desert on the west bank of the Nile."
                }
            };

            await _context.Sites.AddRangeAsync(sites);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {sites.Count} sites successfully.");
        }

        /// <summary>
        /// Seeds all 84 Egyptian artifacts that match the Python AI YOLO model
        /// </summary>
        private async Task SeedArtifactsAsync()
        {
            if (await _context.Artifacts.AnyAsync())
            {
                Console.WriteLine("⏭️  Artifacts already exist. Skipping artifact seeding.");
                return;
            }

            // Get site IDs for assignment
            var egyptianMuseum = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Egyptian Museum (Cairo)"));
            var gem = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Grand Egyptian Museum"));
            var luxorTemple = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Luxor"));
            var giza = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Giza Necropolis"));
            var saqqara = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Saqqara"));
            var dahshur = await _context.Sites.FirstOrDefaultAsync(s => s.Name.Contains("Dahshur"));

            int defaultSiteId = egyptianMuseum?.Id ?? 1;
            int gemSiteId = gem?.Id ?? defaultSiteId;
            int luxorSiteId = luxorTemple?.Id ?? defaultSiteId;
            int gizaSiteId = giza?.Id ?? defaultSiteId;
            int saqqaraSiteId = saqqara?.Id ?? defaultSiteId;
            int dahshurSiteId = dahshur?.Id ?? defaultSiteId;

            // All 84 artifacts matching Python YOLO model ARTIFACT_MAPPING
            var artifacts = new List<Artifact>
            {
                // Class 0-9
                new Artifact { SiteId = defaultSiteId, Name = "Akhenaten", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Sandstone, Limestone", PlaceOfDiscovery = "Karnak Temple, Amarna", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Amenhotep III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite, Granite", PlaceOfDiscovery = "Thebes (Luxor)", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Amenhotep III and Tiye", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Amenhotep III with Plate", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Stone", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Augustus", Category = "Statue", Civilization = "Greco-Roman Egypt", Period = "Ptolemaic Period", Material = "Marble", PlaceOfDiscovery = "Alexandria", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = dahshurSiteId, Name = "Bent Pyramid of King Sneferu", Category = "Pyramid", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Dahshur", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Black Granite Bust of Mentuemhat", Category = "Bust", Civilization = "Ancient Egypt", Period = "Late Period", Material = "Black Granite", PlaceOfDiscovery = "Karnak Temple, Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Bust of Isis", Category = "Bust", Civilization = "Ancient Egypt", Period = "Ptolemaic Period", Material = "Limestone", PlaceOfDiscovery = "Various temples", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Clossal Head of the god Serapis", Category = "Head", Civilization = "Greco-Roman Egypt", Period = "Ptolemaic Period", Material = "Marble", PlaceOfDiscovery = "Alexandria", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Clossal head of Senwosret 1", Category = "Head", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak, Lisht", ImageUrl = "", ImagePublicId = "" },

                // Class 10-19
                new Artifact { SiteId = defaultSiteId, Name = "Coffin of Ahmose I", Category = "Coffin", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Wood, Gold leaf", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Amenhotep III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite, Granite", PlaceOfDiscovery = "Thebes (Luxor)", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of God Ptah", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis, Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Hormoheb", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak Temple", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of King Senwosret IlI", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Middle Kingdom King", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Various sites", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Queen Hatshepsut", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone", PlaceOfDiscovery = "Deir el-Bahari, Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Ramesses II", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis, Tanis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Colossal Statue of Ramesses II beloved of Ptah", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = luxorSiteId, Name = "Colossoi of Memnon", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite", PlaceOfDiscovery = "West Bank, Thebes", ImageUrl = "", ImagePublicId = "" },

                // Class 20-29
                new Artifact { SiteId = defaultSiteId, Name = "Colossus of Senuseret I", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Column of Merenptah", Category = "Architectural Element", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Granite Statue of Osiris", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Abydos", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Granite Statue of Tutankhamun", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite, Quartzite", PlaceOfDiscovery = "Valley of the Kings (KV62), Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = gizaSiteId, Name = "Great Pyramids of Giza", Category = "Pyramid", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone, Granite", PlaceOfDiscovery = "Giza Plateau", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Grey Granite of Ramesses II", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Grey Granite", PlaceOfDiscovery = "Tanis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Hathor Capital", Category = "Architectural Element", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Sandstone", PlaceOfDiscovery = "Dendera Temple", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Hatshepsut face", Category = "Head", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone", PlaceOfDiscovery = "Deir el-Bahari", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Head Statue of Amenhotep III", Category = "Head", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Head Statue of Amenhotep iii", Category = "Head", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },

                // Class 30-39
                new Artifact { SiteId = saqqaraSiteId, Name = "Head of Userkaf", Category = "Head", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Schist", PlaceOfDiscovery = "Saqqara", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Hor I", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Wood, Gold", PlaceOfDiscovery = "Dahshur", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Isis with her child", Category = "Statue", Civilization = "Ancient Egypt", Period = "Late Period", Material = "Bronze", PlaceOfDiscovery = "Various temples", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "King Amenemhat 3", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Fayum", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "King Thutmose III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Schist", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Mask of Thuya", Category = "Funerary Mask", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Gilded Cartonnage, Linen", PlaceOfDiscovery = "Valley of the Kings (KV46), Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = gemSiteId, Name = "Mask of Tutankhamun", Category = "Funerary Mask", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Gold, Lapis Lazuli, Carnelian, Obsidian, Turquoise", PlaceOfDiscovery = "Valley of the Kings (KV62), Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Mask of Yuya", Category = "Funerary Mask", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Gilded Cartonnage, Linen", PlaceOfDiscovery = "Valley of the Kings (KV46), Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Menkaure Statue", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Schist (Greywacke)", PlaceOfDiscovery = "Giza (Valley Temple)", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Mentuhotep Nebhetpre", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Sandstone", PlaceOfDiscovery = "Deir el-Bahari", ImageUrl = "", ImagePublicId = "" },

                // Class 40-49
                new Artifact { SiteId = defaultSiteId, Name = "Naos of Senwosert I", Category = "Shrine", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "White Limestone", PlaceOfDiscovery = "Karnak Temple", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Nefertiti", Category = "Bust", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone, Stucco", PlaceOfDiscovery = "Amarna (Tell el-Amarna)", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Obelsik Tip of Hatshepsut", Category = "Obelisk", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Red Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Offering table of Amenemhat 6", Category = "Relief", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Limestone", PlaceOfDiscovery = "Lisht", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = saqqaraSiteId, Name = "Pyramid of Djoser", Category = "Pyramid", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Saqqara", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Rhetorical Stela of King Ramesses ll", Category = "Stela", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone, Granite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Seated Statue of Amenhotep III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Quartzite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = saqqaraSiteId, Name = "Seated Statue of Djoser", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Diorite", PlaceOfDiscovery = "Saqqara", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Seated Statue of God Sekhmet", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak, Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Seated Statue of Ramesses II", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },

                // Class 50-59
                new Artifact { SiteId = defaultSiteId, Name = "Seated Statue of Ramesses II and God Ptah", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Seated Statue of Thutmose III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Schist", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Senwosret III", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Deir el-Bahari", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = gizaSiteId, Name = "Sphinx", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Giza Plateau", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Sphinx of Amenmhat III", Category = "Sphinx", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Fayum", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Sphinx of Kings Ramesses ll - Merenptah", Category = "Sphinx", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Standing Statue of King Ramses II", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis, Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Standing Statue of Thutmose III", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Schist", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue Head of Akhenaten", Category = "Head", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Sandstone", PlaceOfDiscovery = "Amarna", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Amenhotep III and God Re-Horakhty", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },

                // Class 60-69
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Amenmhat I", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Limestone", PlaceOfDiscovery = "Lisht", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Amun and King", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Ankhesenamun", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Wood, Gold", PlaceOfDiscovery = "Valley of the Kings", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Carcala", Category = "Statue", Civilization = "Greco-Roman Egypt", Period = "Roman Period", Material = "Marble", PlaceOfDiscovery = "Alexandria", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of God Ptah Ramesses ll Goddess Sekhmet", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of God Ra-Horakhty", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Heliopolis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Khafre", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Diorite", PlaceOfDiscovery = "Giza (Valley Temple)", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Khufu", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Ivory", PlaceOfDiscovery = "Abydos", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of King Ramesses ll - Goddess Anath", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Tanis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = gemSiteId, Name = "Statue of King Ramses II Grand Egyptian Museum", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Mit Rahina (Memphis)", ImageUrl = "", ImagePublicId = "" },

                // Class 70-79
                new Artifact { SiteId = luxorSiteId, Name = "Statue of King Ramses II Luxor Temple", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Luxor Temple", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of King Sety Il Holding Standards", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = saqqaraSiteId, Name = "Statue of King Zoser", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Saqqara", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Mentuhotep II", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Sandstone", PlaceOfDiscovery = "Deir el-Bahari", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Merenptah as standard Bearer", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Osiris", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Stone", PlaceOfDiscovery = "Abydos", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Queen Metnoforet", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Limestone", PlaceOfDiscovery = "Thebes", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Ramesses III as standard Bearer", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = dahshurSiteId, Name = "Statue of Snefru", Category = "Statue", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Dahshur", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "Statue of Sobekhotep V", Category = "Statue", Civilization = "Ancient Egypt", Period = "Middle Kingdom", Material = "Granite", PlaceOfDiscovery = "Karnak", ImageUrl = "", ImagePublicId = "" },

                // Class 80-83
                new Artifact { SiteId = gemSiteId, Name = "Statue of Tutankhamun", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Wood, Gold leaf, Stone", PlaceOfDiscovery = "Valley of the Kings (KV62), Luxor", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = dahshurSiteId, Name = "Stela of king Snefero", Category = "Stela", Civilization = "Ancient Egypt", Period = "Old Kingdom", Material = "Limestone", PlaceOfDiscovery = "Dahshur", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "bust of Ramesses II", Category = "Bust", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Memphis", ImageUrl = "", ImagePublicId = "" },
                new Artifact { SiteId = defaultSiteId, Name = "kneeling statue of queen hatshibsut", Category = "Statue", Civilization = "Ancient Egypt", Period = "New Kingdom", Material = "Granite", PlaceOfDiscovery = "Deir el-Bahari", ImageUrl = "", ImagePublicId = "" }
            };

            await _context.Artifacts.AddRangeAsync(artifacts);
            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {artifacts.Count} artifacts successfully.");
        }
    }
}
