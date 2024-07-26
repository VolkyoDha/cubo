using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDashboard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DimDirectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimDirectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DimGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DimMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImdbLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimMovies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactMovies_DimDirectors_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "DimDirectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactMovies_DimGenres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "DimGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactMovies_DimMovies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "DimMovies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactMovies_DirectorId",
                table: "FactMovies",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_FactMovies_GenreId",
                table: "FactMovies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_FactMovies_MovieId",
                table: "FactMovies",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactMovies");

            migrationBuilder.DropTable(
                name: "DimDirectors");

            migrationBuilder.DropTable(
                name: "DimGenres");

            migrationBuilder.DropTable(
                name: "DimMovies");
        }
    }
}
