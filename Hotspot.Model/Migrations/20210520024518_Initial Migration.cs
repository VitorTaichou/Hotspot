using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotspot.Model.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTicketItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(nullable: false),
                    Time = table.Column<long>(nullable: false),
                    Bandwidth = table.Column<string>(nullable: true),
                    Franchise = table.Column<long>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExpireDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTicketItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    ConnectionTime = table.Column<DateTime>(nullable: false),
                    TimeLeft = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coutesy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coutesy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogoutHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    LogoutTime = table.Column<DateTime>(nullable: false),
                    TimeUsed = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogoutHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketsConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultBandwidth = table.Column<string>(nullable: true),
                    DefaultFranchise = table.Column<string>(nullable: true),
                    DefaultIp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Neighborhood = table.Column<string>(nullable: true),
                    LocaleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashFlow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    LocaleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashFlow_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    Rg = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    Rg = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seller_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Commission = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    OneHourQty = table.Column<int>(nullable: false),
                    TwoHourQty = table.Column<int>(nullable: false),
                    ThreeHourQty = table.Column<int>(nullable: false),
                    SixHourQty = table.Column<int>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false),
                    Payment = table.Column<bool>(nullable: false),
                    SellerId = table.Column<int>(nullable: true),
                    CashFlowId = table.Column<int>(nullable: true),
                    EmployeeUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batch_CashFlow_CashFlowId",
                        column: x => x.CashFlowId,
                        principalTable: "CashFlow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Batch_AspNetUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Batch_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatalogBatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Commission = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false),
                    Payment = table.Column<bool>(nullable: false),
                    SellerId = table.Column<int>(nullable: true),
                    CashFlowId = table.Column<int>(nullable: true),
                    EmployeeUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogBatch_CashFlow_CashFlowId",
                        column: x => x.CashFlowId,
                        principalTable: "CashFlow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatalogBatch_AspNetUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatalogBatch_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Phonenumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(nullable: true),
                    EmployeeUserId = table.Column<string>(nullable: true),
                    SellerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phonenumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phonenumber_AspNetUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phonenumber_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Bandwidth = table.Column<string>(nullable: true),
                    Franchise = table.Column<long>(nullable: false),
                    BatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTicket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    Time = table.Column<long>(nullable: false),
                    Bandwidth = table.Column<string>(nullable: true),
                    Franchise = table.Column<long>(nullable: false),
                    ExpirationDays = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    FirstUse = table.Column<bool>(nullable: false),
                    ProfileName = table.Column<string>(nullable: true),
                    BatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogTicket_CatalogBatch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "CatalogBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flow",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    BatchId = table.Column<int>(nullable: true),
                    CatalogBatchId = table.Column<int>(nullable: true),
                    CashFlowId = table.Column<int>(nullable: true),
                    EmployeeUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flow_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flow_CashFlow_CashFlowId",
                        column: x => x.CashFlowId,
                        principalTable: "CashFlow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flow_CatalogBatch_CatalogBatchId",
                        column: x => x.CatalogBatchId,
                        principalTable: "CatalogBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flow_AspNetUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    QtyOneHour = table.Column<int>(nullable: false),
                    QtyTwoHour = table.Column<int>(nullable: false),
                    QtyThreeHour = table.Column<int>(nullable: false),
                    QtySixHour = table.Column<int>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false),
                    PaidValue = table.Column<decimal>(nullable: false),
                    Payment = table.Column<bool>(nullable: false),
                    PaymentMethod = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    SellerId = table.Column<int>(nullable: true),
                    FlowId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Flow_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecialTicket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    FirstUse = table.Column<bool>(nullable: false),
                    SellerId = table.Column<int>(nullable: true),
                    FlowId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialTicket_Flow_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecialTicket_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_LocaleId",
                table: "Address",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_CashFlowId",
                table: "Batch",
                column: "CashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_EmployeeUserId",
                table: "Batch",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_SellerId",
                table: "Batch",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlow_LocaleId",
                table: "CashFlow",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBatch_CashFlowId",
                table: "CatalogBatch",
                column: "CashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBatch_EmployeeUserId",
                table: "CatalogBatch",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBatch_SellerId",
                table: "CatalogBatch",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTicket_BatchId",
                table: "CatalogTicket",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_BatchId",
                table: "Flow",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_CashFlowId",
                table: "Flow",
                column: "CashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_CatalogBatchId",
                table: "Flow",
                column: "CatalogBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Flow_EmployeeUserId",
                table: "Flow",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phonenumber_EmployeeUserId",
                table: "Phonenumber",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phonenumber_SellerId",
                table: "Phonenumber",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EmployeeId",
                table: "Sale",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_FlowId",
                table: "Sale",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_SellerId",
                table: "Sale",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seller_AddressId",
                table: "Seller",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialTicket_FlowId",
                table: "SpecialTicket",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialTicket_SellerId",
                table: "SpecialTicket",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_BatchId",
                table: "Ticket",
                column: "BatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CatalogTicket");

            migrationBuilder.DropTable(
                name: "CatalogTicketItem");

            migrationBuilder.DropTable(
                name: "ConnectionHistory");

            migrationBuilder.DropTable(
                name: "Coutesy");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "LogoutHistory");

            migrationBuilder.DropTable(
                name: "Phonenumber");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "SpecialTicket");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TicketsConfiguration");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Flow");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "CatalogBatch");

            migrationBuilder.DropTable(
                name: "CashFlow");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Locale");
        }
    }
}
