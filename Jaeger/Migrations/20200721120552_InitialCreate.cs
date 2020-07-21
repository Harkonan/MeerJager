using Microsoft.EntityFrameworkCore.Migrations;

namespace MeerJager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Range",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Min = table.Column<int>(nullable: false),
                    Max = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Range", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    ShipID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(nullable: true),
                    HealthId = table.Column<int>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.ShipID);
                    table.ForeignKey(
                        name: "FK_Ships_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ships_Range_HealthId",
                        column: x => x.HealthId,
                        principalTable: "Range",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ships_Range_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Range",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    RangeId = table.Column<int>(nullable: false),
                    DamageId = table.Column<int>(nullable: false),
                    ReloadRounds = table.Column<int>(nullable: false),
                    HitPercent = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Range_DamageId",
                        column: x => x.DamageId,
                        principalTable: "Range",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Weapons_Range_RangeId",
                        column: x => x.RangeId,
                        principalTable: "Range",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MountName = table.Column<string>(nullable: true),
                    ShipId = table.Column<int>(nullable: true),
                    AlwaysExists = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mounts_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeaponToMountMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeaponId = table.Column<int>(nullable: true),
                    MountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponToMountMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponToMountMap_Mounts_MountId",
                        column: x => x.MountId,
                        principalTable: "Mounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponToMountMap_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { 1, "Frigate" });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { 2, "Merchant" });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { 3, "Sloop" });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Id", "ClassName" },
                values: new object[] { 4, "Destroyer" });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 14, 1000, 0 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 13, 110, 80 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 12, 1000, 0 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 11, 110, 80 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 16, 3960, 500 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 15, 10, 5 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 10, 19850, 500 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 9, 50, 20 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 7, 50, 30 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 17, 25, 15 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 6, 94, 11 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 5, 150, 140 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 4, 91, 11 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 3, 130, 120 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 2, 85, 8 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 1, 130, 100 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 8, 150, 90 });

            migrationBuilder.InsertData(
                table: "Range",
                columns: new[] { "Id", "Max", "Min" },
                values: new object[] { 18, 15000, 500 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "ShipID", "ClassId", "HealthId", "Name", "ProfileId" },
                values: new object[] { 1, 4, 1, "Hunt", 2 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "ShipID", "ClassId", "HealthId", "Name", "ProfileId" },
                values: new object[] { 2, 3, 3, "Black Swan", 4 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "ShipID", "ClassId", "HealthId", "Name", "ProfileId" },
                values: new object[] { 3, 1, 5, "Loch", 6 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "ShipID", "ClassId", "HealthId", "Name", "ProfileId" },
                values: new object[] { 4, 2, 7, "Vessel", 8 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 1, 9, 80, "Twin Mounted QF 4-inch Mark XVI", 10, 2, 0 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 2, 15, 80, "Quad Mounted QF 2-pounder Mk. VIII AA", 16, 1, 2 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 3, 11, 80, "Hedgehog Depth Charge", 12, 5, 3 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 6, 11, 90, "Squid Depth Charge", 12, 3, 3 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 4, 13, 90, "Depth Charge", 14, 4, 3 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "DamageId", "HitPercent", "Name", "RangeId", "ReloadRounds", "Type" },
                values: new object[] { 5, 17, 70, "QF 4 inch gun MK V", 18, 4, 0 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 1, false, "Fore Mounted Battery 1", 1 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 16, true, "ASM Rack 3", 3 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 15, true, "ASM Rack 2", 3 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 14, true, "ASM Rack 1", 3 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 13, true, "AA Battery 1", 3 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 12, true, "Aft Battery 1", 3 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 11, true, "AA Mount 6", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 10, true, "AA Mount 5", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 18, false, "Hidden Mount 1", 4 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 9, true, "AA Mount 4", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 7, true, "AA Mount 2", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 6, true, "AA Mount 1", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 5, true, "Rack", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 17, true, "Rack", 1 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 4, false, "Aft Mounted Battery 2", 1 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 3, false, "Aft Mounted Battery 1", 1 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 2, false, "Fore Mounted Battery 2", 1 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 8, true, "AA Mount 3", 2 });

            migrationBuilder.InsertData(
                table: "Mounts",
                columns: new[] { "Id", "AlwaysExists", "MountName", "ShipId" },
                values: new object[] { 19, false, "Hidden Mount 2", 4 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 23, 18, 5 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 22, 18, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 21, 16, 4 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 20, 15, 6 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 19, 14, 6 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 18, 13, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 17, 12, 5 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 16, 11, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 15, 10, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 14, 9, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 25, 19, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 13, 8, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 11, 6, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 10, 5, 4 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 9, 17, 3 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 8, 4, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 4, 4, 1 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 7, 3, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 3, 3, 1 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 6, 2, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 2, 2, 1 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 5, 1, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 12, 7, 2 });

            migrationBuilder.InsertData(
                table: "WeaponToMountMap",
                columns: new[] { "Id", "MountId", "WeaponId" },
                values: new object[] { 24, 19, 5 });

            migrationBuilder.CreateIndex(
                name: "IX_Mounts_ShipId",
                table: "Mounts",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ClassId",
                table: "Ships",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_HealthId",
                table: "Ships",
                column: "HealthId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ProfileId",
                table: "Ships",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_DamageId",
                table: "Weapons",
                column: "DamageId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_RangeId",
                table: "Weapons",
                column: "RangeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponToMountMap_MountId",
                table: "WeaponToMountMap",
                column: "MountId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponToMountMap_WeaponId",
                table: "WeaponToMountMap",
                column: "WeaponId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponToMountMap");

            migrationBuilder.DropTable(
                name: "Mounts");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Range");
        }
    }
}
