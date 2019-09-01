using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace EO.DatabaseContext
{
    public partial class eotestContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public eotestContext()
        {
       
        }

        public eotestContext(DbContextOptions<eotestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<Arrangement> Arrangement { get; set; }
        public virtual DbSet<ArrangementImageMap> ArrangementImageMap { get; set; }
        public virtual DbSet<ArrangementInventoryMap> ArrangementInventoryMap { get; set; }
        public virtual DbSet<ArrangementInventoryInventoryMap> ArrangementInventoryInventoryMap { get; set; }
        public virtual DbSet<Communities> Communities { get; set; }
        public virtual DbSet<Container> Container { get; set; }
        public virtual DbSet<ContainerImageMap> ContainerImageMap { get; set; }
        //public virtual DbSet<ContainerUploads> ContainerUploads { get; set; }
        //public virtual DbSet<GlCodes> GlCodes { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<InventoryContainerMap> InventoryContainerMap { get; set; }
        public virtual DbSet<InventoryImageMap> InventoryImageMap { get; set; }
        public virtual DbSet<InventoryPlantMap> InventoryPlantMap { get; set; }
        public virtual DbSet<InventoryType> InventoryType { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonAddressMap> PersonAddressMap { get; set; }
        public virtual DbSet<Plant> Plant { get; set; }
        public virtual DbSet<PlantImageMap> PlantImageMap { get; set; }
        public virtual DbSet<PlantType> PlantType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<ServiceCode> ServiceCode { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<VendorAddressMap> VendorAddressMap { get; set; }
        public virtual DbSet<WorkOrder> WorkOrder { get; set; }
        public virtual DbSet<WorkOrderInventoryMap> WorkOrderInventoryMap { get; set; }
        public virtual DbSet<Shipment> Shipment { get; set; }
        public virtual DbSet<ShipmentInventoryMap> ShipmentInventoryMap { get; set; }
        public virtual DbSet<PlantName> PlantName { get; set; }
        public virtual DbSet<ContainerName> ContainerName { get; set; }
        public virtual DbSet<ContainerType> ContainerType { get; set; }

        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<MaterialName> MaterialName { get; set; }
        public virtual DbSet<InventoryMaterialMap> InventoryMaterialMap { get; set; }

        public virtual DbSet<Foliage> Foliage { get; set; }
        public virtual DbSet<FoliageType> FoliageType { get; set; }
        public virtual DbSet<FoliageName> FoliageName { get; set; }
        public virtual DbSet<InventoryFoliageMap> InventoryFoliageMap { get; set; }

        private static bool IsPrimaryKey(PropertyInfo property)
        {
            var identityTypes = new List<Type>
            {
             typeof(short),
             typeof(int),
             typeof(long)
            };

            return property.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) && identityTypes.Contains(property.PropertyType);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseMySQL("server=localhost;port=3306;user=EOSystem;password=Orchids@5185;database=eotest");
                //optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=jVW@696969;database=eotest");
            }
        }

        protected void MapTypes(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Iterate over each property found on the Entity class
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    if (property.PropertyInfo == null)
                    {
                        continue;
                    }

                    if (property.IsPrimaryKey() && IsPrimaryKey(property.PropertyInfo))
                    {
                        // At this point we know that the property is a primary key
                        // let's set it to AutoIncrement on insert.
                        modelBuilder.Entity(entityType.ClrType)
                                    .Property(property.Name)
                                    .ValueGeneratedOnAdd()
                                    .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
                    }
                    else if (property.PropertyInfo.PropertyType.IsBoolean())
                    {
                        // Since MySQL stores bool as tinyint, let's add a converter so the tinyint is treated as boolean
                        modelBuilder.Entity(entityType.ClrType)
                                    .Property(property.Name)
                                    .HasConversion(new BoolToZeroOneConverter<short>());
                    }
                }

            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address", "eotest");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AddressTypeId)
                    .HasColumnName("address_type_id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress)
                    .IsRequired()
                    .HasColumnName("street_address")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UnitAptSuite)
                    .HasColumnName("unit_apt_suite")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("address_type", "eotest");

                entity.Property(e => e.AddressTypeId)
                    .HasColumnName("address_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AddressTypeName)
                    .IsRequired()
                    .HasColumnName("address_type_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Arrangement>(entity =>
            {
                entity.ToTable("arrangement", "eotest");

                entity.HasIndex(e => e.ArrangementName)
                    .HasName("fk_person_idx");

                entity.HasIndex(e => e.ServiceCodeId)
                    .HasName("fk_arrangement_service_code_idx");

                entity.Property(e => e.ArrangementId)
                    .HasColumnName("arrangement_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ArrangementName)
                    .IsRequired()
                    .HasColumnName("arrangement_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceCodeId)
                    .HasColumnName("service_code_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.ServiceCode)
                    .WithMany(p => p.Arrangement)
                    .HasForeignKey(d => d.ServiceCodeId)
                    .HasConstraintName("fk_arrangement_service_code");
            });

            modelBuilder.Entity<ArrangementImageMap>(entity =>
            {
                entity.ToTable("arrangement_image_map", "eotest");

                entity.HasIndex(e => e.ArrangmentId)
                    .HasName("fk_arrangement_idx");

                entity.HasIndex(e => e.ImageId)
                    .HasName("fk_image_idx");

                entity.Property(e => e.ArrangementImageMapId)
                    .HasColumnName("arrangement_image_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ArrangmentId)
                    .HasColumnName("arrangment_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ImageId)
                    .HasColumnName("image_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Arrangment)
                    .WithMany(p => p.ArrangementImageMap)
                    .HasForeignKey(d => d.ArrangmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_arrangement");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ArrangementImageMap)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_arrangement_mage");
            });

            modelBuilder.Entity<ArrangementInventoryMap>(entity =>
            {
                entity.ToTable("arrangement_inventory_map", "eotest");

                entity.HasIndex(e => e.ArrangementId)
                    .HasName("fk_arrangemnt_inventory_idx");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_arrangement_idx");

                entity.Property(e => e.ArrangementInventoryMapId)
                    .HasColumnName("arrangement_inventory_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ArrangementId)
                    .HasColumnName("arrangement_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Arrangement)
                    .WithMany(p => p.ArrangementInventoryMap)
                    .HasForeignKey(d => d.ArrangementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_arrangemnt_inventory");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.ArrangementInventoryMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_arrangement");
            });

            modelBuilder.Entity<ArrangementInventoryInventoryMap>(entity =>
            {
                entity.ToTable("arrangement_inventory_inventory_map", "eotest");

                entity.HasIndex(e => e.ArrangementId)
                    .HasName("fk_arrangemnt_inventory_idx");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_arrangement_idx");

                entity.Property(e => e.ArrangementInventoryInventoryMapId)
                    .HasColumnName("arrangement_inventory_inventory_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ArrangementId)
                    .HasColumnName("arrangement_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int unsigned");

                //entity.HasOne(d => d.Arrangement)
                //    .WithMany(p => p.ArrangementInventoryMap)
                //    .HasForeignKey(d => d.ArrangementId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_arrangemnt_arrangemnt");

                //entity.HasOne(d => d.Inventory)
                //    .WithMany(p => p..ArrangementInventoryMap)
                //    .HasForeignKey(d => d.InventoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_arrangement_inventory");
            });

            modelBuilder.Entity<Communities>(entity =>
            {
                entity.HasKey(e => e.CommunityId);

                entity.ToTable("communities", "eotest");

                entity.Property(e => e.CommunityId)
                    .HasColumnName("community_id")
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedNever();

                entity.Property(e => e.CommunityName)
                    .IsRequired()
                    .HasColumnName("community_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Container>(entity =>
            {
                entity.ToTable("container", "eotest");

                entity.Property(e => e.ContainerId)
                    .HasColumnName("container_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerColor)
                    .HasColumnName("container_color")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerTypeName)
                    .HasColumnName("container_type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerSize)
                    .IsRequired()
                    .HasColumnName("container_size")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerLastshipdate).HasColumnName("container_lastshipdate");

                entity.Property(e => e.ContainerName)
                    .IsRequired()
                    .HasColumnName("container_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerPrice)
                    .HasColumnName("container_price")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                     entity.Property(e => e.ContainerSku)
                    .HasColumnName("container_sku")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerTypeId)
                    .HasColumnName("container_type_id")
                     .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerNameId)
                   .HasColumnName("container_name_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerVendor)
                    .HasColumnName("container_vendor")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContainerImageMap>(entity =>
            {
                entity.ToTable("container_image_map", "eotest");

                entity.HasIndex(e => e.ContainerId)
                    .HasName("fk_container_image_idx");

                entity.HasIndex(e => e.ImageId)
                    .HasName("fk_image_contanier_idx");

                entity.Property(e => e.ContainerImageMapId)
                    .HasColumnName("container_image_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerId)
                    .HasColumnName("container_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ImageId)
                    .HasColumnName("image_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Container)
                    .WithMany(p => p.ContainerImageMap)
                    .HasForeignKey(d => d.ContainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_container_image");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ContainerImageMap)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_contanier");
            });

            //modelBuilder.Entity<ContainerUploads>(entity =>
            //{
            //    entity.HasKey(e => e.ContaineruploadId);

            //    entity.ToTable("container_uploads", "eotest");

            //    entity.Property(e => e.ContaineruploadId)
            //        .HasColumnName("containerupload_id")
            //        .HasColumnType("int(11)");

            //    entity.Property(e => e.Filename)
            //        .HasColumnName("filename")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Price)
            //        .HasColumnName("price")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);

            //    entity.Property(e => e.QtyInStock)
            //        .HasColumnName("qty_in_stock")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);
            //});

            //modelBuilder.Entity<GlCodes>(entity =>
            //{
            //    entity.HasKey(e => e.GlCodeId);

            //    entity.ToTable("gl_codes", "eotest");

            //    entity.Property(e => e.GlCodeId)
            //        .HasColumnName("gl_code_id")
            //        .HasColumnType("smallint(5) unsigned");

            //    entity.Property(e => e.GlCode)
            //        .HasColumnName("gl_code")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);

            //    entity.Property(e => e.GlDepartment)
            //        .HasColumnName("gl_department")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);

            //    entity.Property(e => e.GlDescription)
            //        .HasColumnName("gl_description")
            //        .HasMaxLength(45)
            //        .IsUnicode(false);
            //});

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image", "eotest");

                entity.Property(e => e.ImageId)
                    .HasColumnName("image_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ImageData)
                    .IsRequired()
                    .HasColumnName("image_data")
                    .HasColumnType("longblob");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventory", "eotest");

                entity.HasIndex(e => e.ServiceCodeId)
                    .HasName("fk_service_code_idx");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryName)
                    .IsRequired()
                    .HasColumnName("inventory_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.NotifyWhenLowAmount)
                    .HasColumnName("notify_when_low_amount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ServiceCodeId)
                    .HasColumnName("service_code_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryTypeId)
                    .HasColumnName("inventory_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.ServiceCode)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.ServiceCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_service_code");

                entity.HasOne(d => d.InventoryType)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.InventoryTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_type");
            });

            modelBuilder.Entity<InventoryContainerMap>(entity =>
            {
                entity.ToTable("inventory_container_map", "eotest");

                entity.HasIndex(e => e.ContainerId)
                    .HasName("fk_container_inventory_idx");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_container_idx");

                entity.Property(e => e.InventoryContainerMapId)
                    .HasColumnName("inventory_container_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerId)
                    .HasColumnName("container_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Container)
                    .WithMany(p => p.InventoryContainerMap)
                    .HasForeignKey(d => d.ContainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_container_inventory");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InventoryContainerMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_container");
            });

            modelBuilder.Entity<InventoryImageMap>(entity =>
            {
                entity.ToTable("inventory_image_map", "eotest");

                entity.HasIndex(e => e.ImageId)
                    .HasName("fk_image_idx");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_idx");

                entity.Property(e => e.InventoryImageMapId)
                    .HasColumnName("inventory_image_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ImageId)
                    .HasColumnName("image_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.InventoryImageMap)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InventoryImageMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory");
            });
            ////////////////////////////////
            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("plant", "eotest");

                entity.HasIndex(e => e.PlantTypeId)
                    .HasName("fk_plant_type_idx");

                entity.Property(e => e.PlantId)
                    .HasColumnName("plant_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PlantName)
                    .IsRequired()
                    .HasColumnName("plant_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PlantSize)
                    .IsRequired()
                    .HasColumnName("plant_size")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlantTypeId)
                    .HasColumnName("plant_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PlantNameId)
                    .HasColumnName("plant_name_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.PlantType)
                    .WithMany(p => p.Plant)
                    .HasForeignKey(d => d.PlantTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_plant_type");
            });

            modelBuilder.Entity<PlantType>(entity =>
            {
                entity.ToTable("plant_type", "eotest");

                entity.Property(e => e.PlantTypeId)
                    .HasColumnName("plant_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PlantTypeName)
                    .IsRequired()
                    .HasColumnName("plant_type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                //entity.Property(e => e.ImageId)
                //    .HasColumnName("image_id")
                //    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<PlantName>(entity =>
            {
                entity.ToTable("plant_name", "eotest");

                entity.Property(e => e.PlantNameId)
                   .HasColumnName("plant_name_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("plant_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlantTypeId)
                    .HasColumnName("plant_type_id")
                    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<InventoryPlantMap>(entity =>
            {
                entity.ToTable("inventory_plant_map", "eotest");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_plant_idx");

                entity.HasIndex(e => e.PlantId)
                    .HasName("fk_plant_inventory_idx");

                entity.Property(e => e.InventoryPlantMapId)
                    .HasColumnName("inventory_plant_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PlantId)
                    .HasColumnName("plant_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InventoryPlantMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_plant");

                entity.HasOne(d => d.Plant)
                    .WithMany(p => p.InventoryPlantMap)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_plant_inventory");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("material", "eotest");

                entity.HasIndex(e => e.MaterialTypeId)
                    .HasName("fk_material_type_idx");

                entity.Property(e => e.MaterialId)
                    .HasColumnName("material_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.MaterialName)
                    .IsRequired()
                    .HasColumnName("material_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialSize)
                    .IsRequired()
                    .HasColumnName("material_size")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialTypeId)
                    .HasColumnName("material_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.MaterialNameId)
                    .HasColumnName("material_name_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Material)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_material_type");
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.ToTable("material_type", "eotest");

                entity.Property(e => e.MaterialTypeId)
                    .HasColumnName("material_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.MaterialTypeName)
                    .IsRequired()
                    .HasColumnName("material_type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                //entity.Property(e => e.ImageId)
                //    .HasColumnName("image_id")
                //    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<MaterialName>(entity =>
            {
                entity.ToTable("material_name", "eotest");

                entity.Property(e => e.MaterialNameId)
                   .HasColumnName("material_name_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("material_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialTypeId)
                    .HasColumnName("material_type_id")
                    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<InventoryMaterialMap>(entity =>
            {
                entity.ToTable("inventory_material_map", "eotest");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_material2_idx");

                entity.HasIndex(e => e.MaterialId)
                    .HasName("fk_material_inventory2_idx");

                entity.Property(e => e.InventoryMaterialMapId)
                    .HasColumnName("inventory_material_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.MaterialId)
                    .HasColumnName("material_id")
                    .HasColumnType("bigint(20) unsigned");

                //entity.HasOne(d => d.Inventory)
                //    .WithMany(p => p.InventoryMaterialMap)
                //    .HasForeignKey(d => d.InventoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_inventory_material");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.InventoryMaterialMap)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_material_inventory2");
            });

            modelBuilder.Entity<Foliage>(entity =>
            {
                entity.ToTable("foliage", "eotest");

                entity.HasIndex(e => e.FoliageTypeId)
                    .HasName("fk_foliage_type_idx");

                entity.Property(e => e.FoliageId)
                    .HasColumnName("foliage_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.FoliageName)
                    .IsRequired()
                    .HasColumnName("foliage_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.FoliageSize)
                    .IsRequired()
                    .HasColumnName("foliage_size")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FoliageTypeId)
                    .HasColumnName("foliage_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.FoliageNameId)
                    .HasColumnName("foliage_name_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.FoliageType)
                    .WithMany(p => p.Foliage)
                    .HasForeignKey(d => d.FoliageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_foliage_type");
            });

            modelBuilder.Entity<FoliageType>(entity =>
            {
                entity.ToTable("foliage_type", "eotest");

                entity.Property(e => e.FoliageTypeId)
                    .HasColumnName("foliage_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.FoliageTypeName)
                    .IsRequired()
                    .HasColumnName("foliage_type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                //entity.Property(e => e.ImageId)
                //    .HasColumnName("image_id")
                //    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<FoliageName>(entity =>
            {
                entity.ToTable("foliage_name", "eotest");

                entity.Property(e => e.FoliageNameId)
                   .HasColumnName("foliage_name_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("foliage_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FoliageTypeId)
                    .HasColumnName("foliage_type_id")
                    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<InventoryFoliageMap>(entity =>
            {
                entity.ToTable("inventory_foliage_map", "eotest");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_foliage_idx");

                entity.HasIndex(e => e.FoliageId)
                    .HasName("fk_foliage_inventory_idx");

                entity.Property(e => e.InventoryFoliageMapId)
                    .HasColumnName("inventory_foliage_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.FoliageId)
                    .HasColumnName("foliage_id")
                    .HasColumnType("bigint(20) unsigned");

                //entity.HasOne(d => d.Inventory)
                //    .WithMany(p => p.InventoryFoliageMap)
                //    .HasForeignKey(d => d.InventoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_inventory_foliage");

                entity.HasOne(d => d.Foliage)
                    .WithMany(p => p.InventoryFoliageMap)
                    .HasForeignKey(d => d.FoliageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_foliage_inventory");
            });
            /////////////////
            ///
            modelBuilder.Entity<InventoryType>(entity =>
            {
                entity.ToTable("inventory_type", "eotest");

                entity.Property(e => e.InventoryTypeId)
                    .HasColumnName("inventory_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryTypeName)
                    .IsRequired()
                    .HasColumnName("inventory_type_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person", "eotest");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_idx");

                entity.Property(e => e.PersonId)
                    .HasColumnName("person_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PhoneAlt)
                    .HasColumnName("phone_alt")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PhonePrimary)
                    .IsRequired()
                    .HasColumnName("phone_primary")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user");
            });

            modelBuilder.Entity<PersonAddressMap>(entity =>
            {
                entity.ToTable("person_address_map", "eotest");

                entity.HasIndex(e => e.AddresId)
                    .HasName("fk_address_idx");

                entity.HasIndex(e => e.PersonId)
                    .HasName("fk_person_address_idx");

                entity.Property(e => e.PersonAddressMapId)
                    .HasColumnName("person_address_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AddresId)
                    .HasColumnName("addres_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PersonId)
                    .HasColumnName("person_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Addres)
                    .WithMany(p => p.PersonAddressMap)
                    .HasForeignKey(d => d.AddresId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_address");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonAddressMap)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_person");
            });

           

            //modelBuilder.Entity<PlantImageMap>(entity =>
            //{
            //    entity.ToTable("plant_image_map", "eotest");

            //    entity.HasIndex(e => e.ImageId)
            //        .HasName("fk_image_plant_idx");

            //    entity.HasIndex(e => e.PlantId)
            //        .HasName("fk_plant_image_idx");

            //    entity.Property(e => e.PlantImageMapId)
            //        .HasColumnName("plant_image_map_id")
            //        .HasColumnType("bigint(20) unsigned");

            //    entity.Property(e => e.ImageId)
            //        .HasColumnName("image_id")
            //        .HasColumnType("bigint(20) unsigned");

            //    entity.Property(e => e.PlantId)
            //        .HasColumnName("plant_id")
            //        .HasColumnType("bigint(20) unsigned");

            //    entity.HasOne(d => d.Image)
            //        .WithMany(p => p.PlantImageMap)
            //        .HasForeignKey(d => d.ImageId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("fk_image_plant");

            //    entity.HasOne(d => d.Plant)
            //        .WithMany(p => p.PlantImageMap)
            //        .HasForeignKey(d => d.PlantId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("fk_plant_image");
            //});

           

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "eotest");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ServiceCode>(entity =>
            {
                entity.ToTable("service_code", "eotest");

                entity.Property(e => e.ServiceCodeId)
                    .HasColumnName("service_code_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GeneralLedger)
                    .HasColumnName("general_ledger")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(10,0)");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,0)");

                entity.Property(e => e.ServiceCode1)
                    .HasColumnName("service_code")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Taxable)
                    .HasColumnName("taxable")
                    .HasColumnType("bit(1)");
            });

            modelBuilder.Entity<User>(entity =>
                {
                    entity.ToTable("user", "eotest");

                    entity.HasIndex(e => e.RoleId)
                        .HasName("fk_role_idx");

                    entity.Property(e => e.UserId)
                                .HasColumnName("user_id")
                                .HasColumnType("bigint(20) unsigned");

                    entity.Property(e => e.Password)
                                .IsRequired()
                                .HasColumnName("password")
                                .HasMaxLength(45)
                                .IsUnicode(false);

                    entity.Property(e => e.RoleId)
                                .HasColumnName("role_id")
                                .HasColumnType("bigint(20) unsigned");

                    entity.Property(e => e.UserName)
                                .IsRequired()
                                .HasColumnName("user_name")
                                .HasMaxLength(45)
                                .IsUnicode(false);

                    entity.HasOne(d => d.Role)
                                .WithMany(p => p.User)
                                .HasForeignKey(d => d.RoleId)
                                .OnDelete(DeleteBehavior.ClientSetNull)
                                .HasConstraintName("fk_role");
                });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("vendor", "eotest");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendor_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasColumnName("vendor_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.VendorPhone)
                    .IsRequired()
                    .HasColumnName("vendor_phone")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.VendorEmail)
                    .IsRequired()
                    .HasColumnName("vendor_email")
                    .HasMaxLength(45)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<VendorAddressMap>(entity =>
            {
                entity.ToTable("vendor_address_map", "eotest");

                entity.HasIndex(e => e.AddressId)
                    .HasName("fk_address_idx");

                entity.HasIndex(e => e.VendorId)
                    .HasName("fk_person_address_idx");

                entity.Property(e => e.VendorAddressMapId)
                    .HasColumnName("vendor_address_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendor_id")
                    .HasColumnType("bigint(20) unsigned");

                //entity.HasOne(d => d.Address)
                //    .WithMany(p => p.VendorAddressMap)
                //    .HasForeignKey(d => d.AddressId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_address_vendor");

                //entity.HasOne(d => d.Vendor)
                //    .WithMany(p => p.VendorAddressMap)
                //    .HasForeignKey(d => d.VendorId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_vendor_address");
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.ToTable("work_order", "eotest");

                entity.Property(e => e.WorkOrderId)
                    .HasColumnName("work_order_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ClosedDate).HasColumnName("closed_date");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");

                entity.Property(e => e.Paid)
                    .HasColumnName("paid")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.PersonInitiator)
                    .HasColumnName("person_initiator")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PersonReceiver)
                    .HasColumnName("person_receiver")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<WorkOrderInventoryMap>(entity =>
            {
                entity.ToTable("work_order_inventory_map", "eotest");

                entity.HasIndex(e => e.InventoryId)
                    .HasName("fk_inventory_workorder_idx");

                entity.HasIndex(e => e.WorkOrderId)
                    .HasName("fk_workorder_inventory_idx");

                entity.Property(e => e.WorkOrderInventoryMapId)
                    .HasColumnName("work_order_inventory_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.WorkOrderId)
                    .HasColumnName("work_order_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11) unsigned");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.WorkOrderInventoryMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_workorder");

                entity.HasOne(d => d.WorkOrder)
                    .WithMany(p => p.WorkOrderInventoryMap)
                    .HasForeignKey(d => d.WorkOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_workorder_inventory");
            });

            modelBuilder.Entity<VendorAddressMap>(entity => 
            {
                entity.ToTable("vendor_address_map", "eotest");

                entity.HasIndex(e => e.AddressId)
                    .HasName("fk_address_vendor_idx");

                entity.HasIndex(e => e.VendorId)
                    .HasName("fk_vendor_address_idx");

                entity.Property(e => e.VendorAddressMapId)
                    .HasColumnName("vendor_address_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.VendorId)
                    .HasColumnName("vendor_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.HasOne(d => d.Address)
                    .WithMany(d => d.VendorAddressMap)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_address_vendor");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.VendorAddressMap)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_vendor_address");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("shipment", "eotest");

                entity.Property(e => e.ShipmentId)
                    .HasColumnName("shipment_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.VendorId)
                    .IsRequired()
                    .HasColumnName("vendor_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ShipmentDate)
                    .HasColumnName("shipment_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ShipmentInventoryMap>(entity =>
            {
                entity.ToTable("shipment_inventory_map", "eotest");

                entity.Property(e => e.ShipmentInventoryMapId)
                    .HasColumnName("shipment_inventory_map_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ShipmentId)
                   .HasColumnName("shipment_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.InventoryId)
                    .IsRequired()
                    .HasColumnName("inventory_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnName("quantity")
                    .HasColumnType("int unsigned");

                entity.HasOne(e => e.Shipment)
                    .WithMany(d => d.ShipmentInventoryMap)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_shipment_inventory");

                entity.HasOne(e => e.Inventory)
                    .WithMany(e => e.ShipmentInventoryMap)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inventory_shipment");
            });

            modelBuilder.Entity<PlantName>(entity =>
            {
                entity.ToTable("plant_name", "eotest");

                entity.Property(e => e.PlantNameId)
                   .HasColumnName("plant_name_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("plant_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlantTypeId)
                    .HasColumnName("plant_type_id")
                    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<ContainerName>(entity =>
            {
                entity.ToTable("container_name", "eotest");

                entity.Property(e => e.ContainerNameId)
                   .HasColumnName("container_name_id")
                   .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("container_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerTypeId)
                    .HasColumnName("container_type_id")
                    .HasColumnType("bigint(20) unsigned");
            });

            modelBuilder.Entity<ContainerType>(entity =>
            {
                entity.ToTable("container_type", "eotest");

                entity.Property(e => e.ContainerTypeId)
                    .HasColumnName("container_type_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContainerTypeName)
                    .IsRequired()
                    .HasColumnName("container_type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                //entity.Property(e => e.ImageId)
                //    .HasColumnName("image_id")
                //    .HasColumnType("bigint(20) unsigned");
            });

            MapTypes(modelBuilder);
        }
    }
}
