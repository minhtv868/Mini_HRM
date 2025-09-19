INSERT INTO Categories (
    CategoryName, CategoryDesc, ShortName, CategoryUrl, DataTypeId,
    SiteId, MetaTitle, MetaDesc, MetaKeyword, CanonicalTag, H1Tag, SeoFooter,
    ParentCategoryId, CategoryLevel, ImagePath, DisplayOrder, TreeOrder,
    ReviewStatusId, CrUserId, CrDateTime
)
VALUES 
-- 1. Tài chính cá nhân
(N'Tài chính cá nhân', N'Quản lý tiền bạc, chi tiêu, tiết kiệm', 'taichinhcanhan', '/tai-chinh-ca-nhan', 1,
 NULL, N'Tài chính cá nhân cho GenZ', N'Cách quản lý tiền cho sinh viên, người đi làm trẻ', N'tài chính, chi tiêu, tiết kiệm', NULL, N'Tài chính cá nhân', NULL,
 NULL, 1, '/images/category/taichinh.jpg', 1, 1,
 1, 1, GETDATE()),

-- 2. Đầu tư
(N'Đầu tư', N'Cổ phiếu, trái phiếu, bất động sản, crypto...', 'dautu', '/dau-tu', 1,
 NULL, N'Kiến thức đầu tư cơ bản', N'Học đầu tư từ con số 0', N'dầu tư, chứng khoán, crypto', NULL, N'Đầu tư thông minh', NULL,
 NULL, 1, '/images/category/dautu.jpg', 2, 2,
 1, 1, GETDATE()),

-- 3. Kiến thức tài chính
(N'Kiến thức tài chính', N'Hiểu các khái niệm về tài chính, kinh tế', 'kienthuctaichinh', '/kien-thuc-tai-chinh', 1,
 NULL, N'Kiến thức tài chính dễ hiểu', N'Hướng dẫn các khái niệm tài chính cơ bản', N'tài chính, giáo dục tài chính', NULL, N'Hiểu về tài chính', NULL,
 NULL, 1, '/images/category/kienthuc.jpg', 3, 3,
 1, 1, GETDATE()),

-- 4. Tin tức kinh tế
(N'Tin tức kinh tế', N'Tổng hợp tin tức mới nhất về thị trường', 'tintuckinhte', '/tin-tuc-kinh-te', 1,
 NULL, N'Tin tức kinh tế cập nhật', N'Thị trường chứng khoán, giá vàng, tỷ giá', N'tin tức, kinh tế, thị trường', NULL, N'Tin tức thị trường', NULL,
 NULL, 1, '/images/category/tintuc.jpg', 4, 4,
 1, 1, GETDATE()),

-- 5. Công nghệ tài chính (Fintech)
(N'Fintech', N'Công nghệ ứng dụng trong tài chính, ví điện tử, ngân hàng số', 'fintech', '/fintech', 1,
 NULL, N'Fintech tại Việt Nam', N'Các app tài chính, ngân hàng số', N'fintech, ngân hàng số, ví điện tử', NULL, N'Công nghệ tài chính', NULL,
 NULL, 1, '/images/category/fintech.jpg', 5, 5,
 1, 1, GETDATE()),

-- 6. Ngân hàng & Thẻ
(N'Ngân hàng & Thẻ', N'So sánh thẻ tín dụng, thẻ ghi nợ và ngân hàng số', 'nganhang', '/ngan-hang-the', 1,
 NULL, N'So sánh ngân hàng và thẻ tín dụng', N'Nên dùng ngân hàng nào? Ưu đãi thẻ tín dụng mới nhất', N'ngân hàng, thẻ tín dụng, thẻ ghi nợ', NULL, N'Ngân hàng & Thẻ', NULL,
 NULL, 1, '/images/category/nganhang.jpg', 6, 6, 1, 1, GETDATE()),

-- 7. Vay tiêu dùng
(N'Vay tiêu dùng', N'Vay tín chấp, mua trước trả sau, BNPL', 'vaytieudung', '/vay-tieu-dung', 1,
 NULL, N'Vay tiêu dùng uy tín', N'Các hình thức vay cá nhân, lãi suất thấp', N'vay tiền, tín dụng cá nhân, BNPL', NULL, N'Vay tiêu dùng thông minh', NULL,
 NULL, 1, '/images/category/vay.jpg', 7, 7, 1, 1, GETDATE()),

-- 8. Sản phẩm tài chính
(N'Sản phẩm tài chính', N'Các sản phẩm đầu tư, bảo hiểm, tiết kiệm', 'sanphamtc', '/san-pham-tai-chinh', 1,
 NULL, N'Tìm hiểu sản phẩm tài chính phổ biến', N'Bảo hiểm, đầu tư, tích lũy online', N'tài chính, sản phẩm, bảo hiểm', NULL, N'Sản phẩm tài chính', NULL,
 NULL, 1, '/images/category/sanpham.jpg', 8, 8, 1, 1, GETDATE()),

-- 9. Khởi nghiệp tài chính
(N'Khởi nghiệp tài chính', N'Kinh doanh, kiếm tiền, side hustle', 'khoinghiep', '/khoi-nghiep-tai-chinh', 1,
 NULL, N'GenZ khởi nghiệp tài chính', N'Thủ thuật kiếm tiền từ con số 0', N'khởi nghiệp, tài chính cá nhân, làm thêm', NULL, N'Làm giàu thời GenZ', NULL,
 NULL, 1, '/images/category/khoinghiep.jpg', 9, 9, 1, 1, GETDATE()),

-- 10. Ví điện tử & App tài chính
(N'Ví điện tử & App', N'MoMo, ZaloPay, Cake, Timo, MB...', 'app', '/vi-dien-tu-app', 1,
 NULL, N'Ứng dụng tài chính nên dùng', N'Review ví điện tử, app ngân hàng miễn phí', N'app tài chính, ví điện tử, chuyển tiền', NULL, N'App tài chính tốt nhất', NULL,
 NULL, 1, '/images/category/app.jpg', 10, 10, 1, 1, GETDATE()),

-- 11. Kỹ năng chi tiêu
(N'Kỹ năng chi tiêu', N'Mẹo tiết kiệm, lập ngân sách, quản lý ví', 'chitieu', '/ky-nang-chi-tieu', 1,
 NULL, N'Chi tiêu thông minh', N'Tips chi tiêu hiệu quả cho sinh viên và người đi làm', N'tiết kiệm, quản lý tiền, ngân sách', NULL, N'Tips chi tiêu thông minh', NULL,
 NULL, 1, '/images/category/chitieu.jpg', 11, 11, 1, 1, GETDATE()),

-- 12. Tỷ giá, lãi suất
(N'Tỷ giá & Lãi suất', N'Tỷ giá USD, lãi suất ngân hàng, vàng...', 'tygia', '/ty-gia-lai-suat', 1,
 NULL, N'Tỷ giá & thị trường lãi suất', N'Cập nhật tỷ giá USD, EUR, lãi suất gửi tiết kiệm', N'tỷ giá, lãi suất, thị trường', NULL, N'Cập nhật tỷ giá', NULL,
 NULL, 1, '/images/category/tygia.jpg', 12, 12, 1, 1, GETDATE()),

-- 13. Bảo hiểm & An sinh
(N'Bảo hiểm & An sinh', N'Bảo hiểm y tế, nhân thọ, thất nghiệp', 'baohiem', '/bao-hiem-an-sinh', 1,
 NULL, N'Tìm hiểu bảo hiểm và quyền lợi', N'Cần gì khi mua bảo hiểm? Những điều cần biết', N'bảo hiểm, an sinh xã hội, sức khỏe', NULL, N'Bảo vệ tài chính cá nhân', NULL,
 NULL, 1, '/images/category/baohiem.jpg', 13, 13, 1, 1, GETDATE()),

-- 14. So sánh dịch vụ
(N'So sánh dịch vụ', N'Bảng so sánh ngân hàng, ví, app tài chính', 'sosanh', '/so-sanh-dich-vu', 1,
 NULL, N'So sánh sản phẩm tài chính', N'So sánh ngân hàng miễn phí, thẻ hoàn tiền tốt nhất', N'so sánh, ngân hàng, dịch vụ tài chính', NULL, N'So sánh lựa chọn tốt nhất', NULL,
 NULL, 1, '/images/category/sosanh.jpg', 14, 14, 1, 1, GETDATE()),

-- 15. Thuế & pháp lý tài chính
(N'Thuế & pháp lý', N'Thuế thu nhập cá nhân, hóa đơn, pháp lý liên quan tài chính', 'thue', '/thue-phap-ly', 1,
 NULL, N'Hướng dẫn về thuế tài chính', N'Nộp thuế TNCN, pháp luật liên quan tiền bạc', N'thuế, tài chính, pháp lý', NULL, N'Thuế và quy định tài chính', NULL,
 NULL, 1, '/images/category/thue.jpg', 15, 15, 1, 1, GETDATE());



 --- Site
 CREATE TABLE Sites (
    SiteId SMALLINT IDENTITY(1,1) PRIMARY KEY,
    SiteName NVARCHAR(200) NOT NULL,
    ShortName NVARCHAR(50) NULL,
    WebsiteDomain NVARCHAR(255) NULL,
    Logo NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    DisplayOrder SMALLINT NULL,
    CrUserId INT NULL,
    CrDateTime DATETIME NOT NULL DEFAULT GETDATE(),
    UpdUserId INT NULL,
    UpdDateTime DATETIME NULL
);
INSERT INTO Sites (SiteName, ShortName, WebsiteDomain, Logo, IsActive, DisplayOrder, CrUserId, CrDateTime)
VALUES 
(N'Bóng đá 24h', N'Bóng đá', 'ngoaihanganh.vn', '/images/sites/bd24h.png', 1, 1, 1, GETDATE()),
(N'Livescore', N'LS', 'livescore.com', '/images/sites/livescore.png', 1, 2, 1, GETDATE());


-- League
CREATE TABLE League (
    LeagueId SMALLINT PRIMARY KEY,
    LeagueCode NVARCHAR(50),
    LeagueNameSMS NVARCHAR(100),
    LeagueAlias NVARCHAR(100),
    LeagueName NVARCHAR(255) NOT NULL DEFAULT(''),
    LeagueDesc NVARCHAR(500) NOT NULL DEFAULT(''),
    LeagueImage NVARCHAR(255),
    LeagueOwnerId SMALLINT NULL,
    RelegationNum TINYINT NULL,
    SortOrder SMALLINT NULL,
    LivescoreNames NVARCHAR(255),
    StatusId TINYINT NULL,
    LeagueIndex TINYINT NULL,
    LeagueUrl NVARCHAR(255),
    LeagueArea NVARCHAR(100),
    MainColor NVARCHAR(50),
    ContrastColor NVARCHAR(50),
    EstablishTime DATETIME NULL,
    EstablishText NVARCHAR(100),
    TotalClub NVARCHAR(50),
    EliminationRoundFor NVARCHAR(50),
    LeagueRelate NVARCHAR(255),
    CurrentChampionTeam NVARCHAR(100),
    BestTeam NVARCHAR(100),
    TelevisionList NVARCHAR(255),
    LinkFacebook NVARCHAR(255),
    LinkTwitter NVARCHAR(255),
    LinkYoutube NVARCHAR(255),
    LinkInstagram NVARCHAR(255),
    Website NVARCHAR(255),
    VideoTagId INT NULL,
    LiveTagId INT NULL,
    BiographyArticleId INT NULL,
    HomeCountry NVARCHAR(100),
    StartToEnd NVARCHAR(50),
    PlayArea NVARCHAR(100),
    BannerImage NVARCHAR(255),
    IsBXH TINYINT NOT NULL DEFAULT(1),
    DisplayType TINYINT NULL,
    Featured TINYINT NULL,
    LeagueType TINYINT NULL,
    SiteId INT NULL,
    CrUserId INT NULL,
    CrDateTime DATETIME NOT NULL DEFAULT(GETDATE()),
    UpdUserId INT NULL,
    UpdDateTime DATETIME NULL,
    LastUpdateTime DATETIME NULL
);
INSERT INTO League (LeagueId, LeagueCode, LeagueAlias, LeagueName, LeagueArea, HomeCountry, LeagueType, IsBXH, CrDateTime)
VALUES
(1, 'WC', 'world-cup', N'FIFA World Cup', N'World', N'FIFA', 1, 1, GETDATE()),
(2, 'EURO', 'euro', N'UEFA European Championship', N'Europe', N'UEFA', 1, 1, GETDATE()),
(3, 'C1', 'ucl', N'UEFA Champions League', N'Europe', N'UEFA', 1, 1, GETDATE()),
(4, 'C2', 'uel', N'UEFA Europa League', N'Europe', N'UEFA', 1, 1, GETDATE()),
(5, 'C3', 'uecl', N'UEFA Europa Conference League', N'Europe', N'UEFA', 1, 1, GETDATE()),
(6, 'EPL', 'premier-league', N'English Premier League', N'Europe', N'England', 1, 1, GETDATE()),
(7, 'FAC', 'fa-cup', N'FA Cup', N'Europe', N'England', 2, 1, GETDATE()),
(8, 'LC', 'efl-cup', N'EFL Cup (Carabao Cup)', N'Europe', N'England', 2, 1, GETDATE()),
(9, 'CH', 'championship', N'EFL Championship', N'Europe', N'England', 2, 1, GETDATE()),
(10, 'L1', 'league-one', N'League One', N'Europe', N'England', 2, 1, GETDATE()),
(11, 'L2', 'league-two', N'League Two', N'Europe', N'England', 2, 1, GETDATE()),
(12, 'SPA1', 'la-liga', N'La Liga', N'Europe', N'Spain', 1, 1, GETDATE()),
(13, 'SPA2', 'segunda', N'Segunda División', N'Europe', N'Spain', 2, 1, GETDATE()),
(14, 'COPA', 'copa-del-rey', N'Copa del Rey', N'Europe', N'Spain', 2, 1, GETDATE()),
(15, 'ITA1', 'serie-a', N'Serie A', N'Europe', N'Italy', 1, 1, GETDATE()),
(16, 'ITA2', 'serie-b', N'Serie B', N'Europe', N'Italy', 2, 1, GETDATE()),
(17, 'COPITA', 'coppa-italia', N'Coppa Italia', N'Europe', N'Italy', 2, 1, GETDATE()),
(18, 'GER1', 'bundesliga', N'Bundesliga', N'Europe', N'Germany', 1, 1, GETDATE()),
(19, 'GER2', 'bundesliga-2', N'2. Bundesliga', N'Europe', N'Germany', 2, 1, GETDATE()),
(20, 'DFB', 'dfb-pokal', N'DFB Pokal', N'Europe', N'Germany', 2, 1, GETDATE()),
(21, 'FRA1', 'ligue-1', N'Ligue 1', N'Europe', N'France', 1, 1, GETDATE()),
(22, 'FRA2', 'ligue-2', N'Ligue 2', N'Europe', N'France', 2, 1, GETDATE()),
(23, 'CDF', 'coupe-de-france', N'Coupe de France', N'Europe', N'France', 2, 1, GETDATE()),
(24, 'NED1', 'eredivisie', N'Eredivisie', N'Europe', N'Netherlands', 1, 1, GETDATE()),
(25, 'POR1', 'liga-portugal', N'Primeira Liga', N'Europe', N'Portugal', 1, 1, GETDATE()),
(26, 'BEL1', 'jupiler-pro-league', N'Belgian Pro League', N'Europe', N'Belgium', 1, 1, GETDATE()),
(27, 'TUR1', 'super-lig', N'Süper Lig', N'Europe', N'Turkey', 1, 1, GETDATE()),
(28, 'RUS1', 'rpl', N'Russian Premier League', N'Europe', N'Russia', 1, 1, GETDATE()),
(29, 'BRA1', 'serie-a-br', N'Brasileirão Serie A', N'South America', N'Brazil', 1, 1, GETDATE()),
(30, 'BRA2', 'serie-b-br', N'Brasileirão Serie B', N'South America', N'Brazil', 2, 1, GETDATE()),
(31, 'ARG1', 'liga-profesional', N'Liga Profesional Argentina', N'South America', N'Argentina', 1, 1, GETDATE()),
(32, 'ARGC', 'copa-argentina', N'Copa Argentina', N'South America', N'Argentina', 2, 1, GETDATE()),
(33, 'UCLUB', 'copa-libertadores', N'Copa Libertadores', N'South America', N'CONMEBOL', 1, 1, GETDATE()),
(34, 'UCUP', 'copa-sudamericana', N'Copa Sudamericana', N'South America', N'CONMEBOL', 2, 1, GETDATE()),
(35, 'MLS', 'mls', N'Major League Soccer', N'North America', N'USA', 1, 1, GETDATE()),
(36, 'USOC', 'us-open-cup', N'US Open Cup', N'North America', N'USA', 2, 1, GETDATE()),
(37, 'MEX1', 'liga-mx', N'Liga MX', N'North America', N'Mexico', 1, 1, GETDATE()),
(38, 'AFC1', 'afc-champions', N'AFC Champions League', N'Asia', N'AFC', 1, 1, GETDATE()),
(39, 'ACL', 'afc-cup', N'AFC Cup', N'Asia', N'AFC', 2, 1, GETDATE()),
(40, 'JPN1', 'j-league', N'J1 League', N'Asia', N'Japan', 1, 1, GETDATE()),
(41, 'KOR1', 'k-league', N'K League 1', N'Asia', N'South Korea', 1, 1, GETDATE()),
(42, 'KOR2', 'k-league-2', N'K League 2', N'Asia', N'South Korea', 2, 1, GETDATE()),
(43, 'CHN1', 'csl', N'Chinese Super League', N'Asia', N'China', 1, 1, GETDATE()),
(44, 'AUS1', 'a-league', N'A-League', N'Oceania', N'Australia', 1, 1, GETDATE()),
(45, 'AFCON', 'africa-cup', N'Africa Cup of Nations', N'Africa', N'CAF', 1, 1, GETDATE()),
(46, 'CAFCL', 'caf-champions', N'CAF Champions League', N'Africa', N'CAF', 1, 1, GETDATE()),
(47, 'CAFCC', 'caf-confed-cup', N'CAF Confederation Cup', N'Africa', N'CAF', 2, 1, GETDATE()),
(48, 'WCQ', 'world-cup-qual', N'World Cup Qualification', N'World', N'FIFA', 2, 1, GETDATE()),
(49, 'ASIAQ', 'afc-asian-qual', N'AFC Asian Cup Qualification', N'Asia', N'AFC', 2, 1, GETDATE()),
(50, 'ASEAN', 'aff-cup', N'AFF Championship (Suzuki Cup)', N'Asia', N'Southeast Asia', 1, 1, GETDATE());


-- Team
CREATE TABLE Teams (
    TeamId SMALLINT PRIMARY KEY,
    TeamName NVARCHAR(200) NOT NULL,
    TeamCode NVARCHAR(50),
    VNName NVARCHAR(200),
    NumberPlayer TINYINT NULL,
    CountryId SMALLINT NULL,
    Website NVARCHAR(200),
    LogoPath NVARCHAR(200),
    MainColor NVARCHAR(50),
    ContrastColor NVARCHAR(50),
    LeagueId SMALLINT NULL,
    RootTeamId SMALLINT NULL,
    StadiumId SMALLINT NULL,
    IsMainTeam TINYINT NULL, -- 1 = chính, 0 = phụ
    StatusId TINYINT NULL,
    SortOrder SMALLINT NULL,
    TeamUrl NVARCHAR(200),
    TeamTypeId TINYINT NULL,
    CaptainName NVARCHAR(100),
    CaptainArticleId INT NULL,
    CoachName NVARCHAR(100),
    CoachArticleId INT NULL,
    SiteId INT NULL,
    CrUserId INT NULL,
    CrDateTime DATETIME DEFAULT GETDATE(),
    UpdUserId INT NULL,
    UpdDateTime DATETIME NULL,
    LastUpdateTime DATETIME NULL
);
