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
