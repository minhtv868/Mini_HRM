/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * CKEditor 4 LTS ("Long Term Support") is available under the terms of the Extended Support Model.
 */

// Register a templates definition set named "default".
CKEDITOR.addTemplates( 'default', {
	// The name of sub folder which hold the shortcut preview images of the
	// templates.
	imagesPath: CKEDITOR.getUrl( CKEDITOR.plugins.getPath( 'templates' ) + 'templates/images/' ),

	// The templates definitions.
	templates: [ {
		title: 'Kiểu 1 - 1:1',
		image: 'album-kieu-1.gif',
		description: '2 ảnh dọc 1:2',
		html: `<div class="block-photo">
				 <div class="div-photo div-photo-grid2">
					<figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
				 </div>`
	},
	{
		title: 'Kiểu 2 - 2:1',
		image: 'album-kieu-2.gif',
		description: '2 ảnh vuông',
		html: `<div class="block-photo">
				 <div class="div-photo div-photo-grid2">
					<figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
				 </div>`
	},
	{
		title: 'Kiểu 3 - 8:9',
		image: 'album-kieu-3.gif',
		description: '2 ảnh ngang 16:9 ',
		html: `<div class="block-photo">
				 <div class="div-photo">
					<figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
				 </div>`
		},
		{
			title: 'Kiểu 4 - 3:2',
			image: 'album-kieu-4.gif',
			description: '3 ảnh dọc 1:2',
			html: `<div class="block-photo">
				 <div class="div-photo">
					<figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
				 </div>`
		},
		{
			title: 'Kiểu 5 - 3:2',
			image: 'album-kieu-5.gif',
			description: '<b>3 ảnh vuông</b><br/> <span>1 ảnh to trái, 2 ảnh nhỏ phải</span>',
			html: `<div class="block-photo">
						 <div class="flex-photo">
						 <div class="col-photo-1">
								<figure>
									<div class="thumb-photo thumb-photo-1x1">
										<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
									</div>
								</figure>
						</div>
						<div class="col-photo-2">
							<figure>
								<div class="thumb-photo thumb-photo-1x1">
									<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
								</div>
							</figure>
							 <figure>
								<div class="thumb-photo thumb-photo-1x1">
									<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
								</div>
							</figure>
						</div>
						</div>
						<figure>
							<figcaption></figcaption>
						</figure>
						</div>`
		},
		{
			title: 'Kiểu 6 - 16:13.5',
			image: 'album-kieu-6.gif',
			description: '3 ảnh ngang 16:9 : 2 ảnh nhỏ trên, 1 ảnh to dưới',
			html: `<div class="block-photo">
				 <div class="div-photo div-photo-grid2">
					<figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
					 <div class="div-photo">
					<figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
					 </div>`
		},
		{
			title: 'Kiểu 7 - 1:2',
			image: 'album-kieu-7.gif',
			description: '4 ảnh dọc 1:2',
			html: `<div class="block-photo">
				 <div class="div-photo div-photo-grid4">
					<figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x2">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						 </div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
					 </div>`
		},
		{
			title: 'Kiểu 8 - 1:1',
			image: 'album-kieu-8.gif',
			description: '',
			html: `<div class="block-photo">
			<div class="div-photo div-photo-grid2">
					<figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo thumb-photo-1x1">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
			</div>`
		},
		{
			title: 'Kiểu 9 - 16:9',
			image: 'album-kieu-9.gif',
			description: '',
			html: `<div class="block-photo">
				 <div class="div-photo div-photo-grid2">
					<figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/1.jpg" alt="">
						</div>
					</figure>
					 <figure>
						<div class="thumb-photo">
							<img src="http://design.icsoft.vn/Thumbnail/html/images/2.png" alt="">
						</div>
					</figure>
				</div>
				<figure>
					<figcaption></figcaption>
				</figure>
				</div>`
		}
	]
} );
