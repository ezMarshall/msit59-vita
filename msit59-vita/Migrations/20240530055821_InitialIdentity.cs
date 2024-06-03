using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace msit59_vita.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerNickName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CustomerEmail = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CustomerCellPhone = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    CustomerLocalPhone = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    CustomerAddressCity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CustomerAddressDistrict = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CustomerAddressDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerAddressMemo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerEinvoiceNumber = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    CustomerPassword = table.Column<string>(type: "nvarchar(512)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StoreAddressCity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StoreAddressDistrict = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StoreAddressDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreAddressMemo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorePhoneNumber = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    StoreImage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StoreUniformInvoiceVia = table.Column<bool>(type: "bit", nullable: false),
                    StoreAccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StorePassword = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    StoreSetTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    StoreOrderStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    StoreOnService = table.Column<byte>(type: "tinyint", nullable: false),
                    StoreEndServiceTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    StoreLinePay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Favorites",
                columns: table => new
                {
                    FavoriteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    StoreID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteID);
                    table.ForeignKey(
                        name: "FK_Favorites_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Favorites_Stores",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    PredictedArrivalTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    OrderDeliveryVia = table.Column<bool>(type: "bit", nullable: false),
                    OrderPhoneNumber = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    OrderStoreMemo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderPayment = table.Column<bool>(type: "bit", nullable: false),
                    OrderUniformInvoiceVia = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)0),
                    OrderEinvoiceNumber = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    OrderAddressCity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OrderAddressDistrict = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OrderAddressDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderAddressMemo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerOrderStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderFinishedTime = table.Column<DateTime>(type: "smalldatetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Orders_Stores",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryOnDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Stores",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "StoreOpeningHours",
                columns: table => new
                {
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    MyWeekDay = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StoreOpenOrNot = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    StoreOpeningTime = table.Column<TimeOnly>(type: "time", nullable: true, defaultValueSql: "('')"),
                    StoreClosingTime = table.Column<TimeOnly>(type: "time", nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOpeningHours", x => new { x.StoreID, x.MyWeekDay });
                    table.ForeignKey(
                        name: "FK_StoreOpeningHours_Stores",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "OrderMessages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MessageInformedTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    MessageContent = table.Column<byte>(type: "tinyint", nullable: false),
                    MessageStatus = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMessages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_OrderMessages_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ReviewTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ReviewRating = table.Column<byte>(type: "tinyint", nullable: false),
                    ReviewContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StoreReplyTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    StoreReplyContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    ProductUnitPrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    ProductUnitsInStock = table.Column<short>(type: "smallint", nullable: false),
                    ProductOnSell = table.Column<bool>(type: "bit", nullable: false),
                    ProductImage = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories",
                        column: x => x.CategoryID,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Products_Stores",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Details", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_Order_Details_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_Order_Details_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    ShoppingCartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartQuantity = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.ShoppingCartID);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

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
                name: "IX_Favorites_CustomerID",
                table: "Favorites",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_StoreID",
                table: "Favorites",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMessages_OrderID",
                table: "OrderMessages",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreID",
                table: "Orders",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_StoreID",
                table: "ProductCategories",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreID",
                table: "Products",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderID",
                table: "Reviews",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerID",
                table: "ShoppingCarts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductID",
                table: "ShoppingCarts",
                column: "ProductID");
        }

        /// <inheritdoc />
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
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderMessages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "StoreOpeningHours");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
