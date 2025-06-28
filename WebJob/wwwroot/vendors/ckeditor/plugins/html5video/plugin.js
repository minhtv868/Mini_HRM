CKEDITOR.plugins.add( 'html5video', {
    requires: 'widget',
    lang: 'bg,ca,de,en,eu,es,ru,uk,fr,ko,pt,pt-br,pl,vi',
    icons: 'html5video',
    init: function( editor ) {
        editor.widgets.add( 'html5video', {
            button: editor.lang.html5video.button,
            template: '<div class="ckeditor-html5-video"></div>',
            /*
             * Allowed content rules (http://docs.ckeditor.com/#!/guide/dev_allowed_content_rules):
             *  - div-s with text-align,float,margin-left,margin-right inline style rules and required ckeditor-html5-video class.
             *  - video tags with src, controls, width and height attributes.
             */
            allowedContent: 'figure(*);figcaption;div[data-responsive](!ckeditor-html5-video){text-align,float,margin-left,margin-right}; video[src,poster,controls,autoplay,width, height,loop]{max-width,height};',
            requiredContent: 'div(ckeditor-html5-video); video[src];',
            upcast: function( element ) {
                return element.name === 'div' && element.hasClass( 'ckeditor-html5-video' );
            },
            dialog: 'html5video',
            init: function() {
                var src = '';
                var autoplay = '';
                var caption = '';
                var loop = '';
                var controls = '';
                var align = this.element.getStyle( 'text-align' );

                var width = '';
                var height = '';
                var poster = '';
                var videoElement = this.element.findOne('video');

                // If there's a child (the video element)
                if (videoElement ) {
                    // get it's attributes.
                    src = videoElement.getAttribute( 'src' );
                    width = videoElement.getAttribute( 'width' );
                    height = videoElement.getAttribute( 'height' );
                    autoplay = this.element.getChild(0).getAttribute('autoplay');
                    allowdownload = !videoElement.getAttribute( 'controlslist' );
                    loop = videoElement.getAttribute( 'loop' );
                    advisorytitle = videoElement.getAttribute( 'title' );
                    controls = this.element.getChild(0).getAttribute('controls');
					responsive = this.element.getAttribute( 'data-responsive' );
                    poster = this.element.getChild(0).getAttribute('poster');
                    var figcaption = this.element.findOne('figcaption');
                    if (figcaption) {
                        caption = figcaption.getText();
                    }
                }

                if ( src ) {
                    this.setData('src', src);

                    if (caption) {
                        this.setData('caption', caption);
                    }

                    if ( align ) {
                        this.setData( 'align', align );
                    } else {
                        this.setData( 'align', 'none' );
                    }

                    if ( width ) {
                        this.setData( 'width', width );
                    }

                    if ( height ) {
                        this.setData( 'height', height );
                    }

                    if ( autoplay ) {
                        this.setData( 'autoplay', 'yes' );
                    }

                    if ( allowdownload ) {
                        this.setData( 'allowdownload', 'yes' );
                    }

                    if ( loop ) {
                        this.setData( 'loop', 'yes' );
                    }
								
                    if ( advisorytitle ) {
                        this.setData( 'advisorytitle', advisorytitle );
                    }
		
                    if ( responsive ) {
                        this.setData( 'responsive', responsive );	
                    }

                    if (controls) {
                        this.setData('controls', controls);
                    }

                    //if ( poster ) {
                        this.setData('poster', poster);
                    //}
                }
            },
            data: function () {
                var videoElement = this.element.findOne('video');

                // If there is an video source
                if (this.data.src) {

                    var figureElement = this.element.findOne('figure');
                    if (!figureElement) {
                        figure = new CKEDITOR.dom.element('figure');
                        this.element.append(figure);
                    }

                    // and there isn't a child (the video element)
                    if (!videoElement) {

                        // Create a new <video> element.
                        var videoElement = new CKEDITOR.dom.element( 'video' );
                        // Set the controls attribute.
                        if (this.data.controls) {
                            videoElement.setAttribute('controls', 'controls');
                        }
                        // Append it to the container of the plugin.
                        //this.element.append(videoElement);
                        this.element.findOne('figure').append(videoElement);
                        captionElement = new CKEDITOR.dom.element('figcaption');
                        captionElement.appendText(this.data.caption);
                        this.element.findOne('figure').append(captionElement);
                    }
                    videoElement.setAttribute('src', this.data.src);
                    //videoElement.setAttribute( 'src', this.data.src );
                    if (this.data.width) videoElement.setAttribute( 'width', this.data.width );
                    if (this.data.height) videoElement.setAttribute( 'height', this.data.height );

                    if ( this.data.responsive ) {
                        this.element.setAttribute("data-responsive", this.data.responsive);
                        videoElement.setStyle( 'max-width', '100%' );
                        videoElement.setStyle( 'height', 'auto' );
                    } else {
			            this.element.removeAttribute("data-responsive");
                        videoElement.removeStyle( 'max-width' );
                        videoElement.removeStyle( 'height' );
                    }

                    if (this.data.poster)
                        videoElement.setAttribute('poster', this.data.poster);	
                    else videoElement.removeAttribute('poster');	
                }

                this.element.removeStyle( 'float' );
                this.element.removeStyle( 'margin-left' );
                this.element.removeStyle( 'margin-right' );

                if ( this.data.align === 'none' ) {
                    this.element.removeStyle( 'text-align' );
                } else {
                    this.element.setStyle( 'text-align', this.data.align );
                }

                if ( this.data.align === 'left' ) {
                    this.element.setStyle( 'float', this.data.align );
                    this.element.setStyle( 'margin-right', '10px' );
                } else if ( this.data.align === 'right' ) {
                    this.element.setStyle( 'float', this.data.align );
                    this.element.setStyle( 'margin-left', '10px' );
                }

                if (videoElement ) {
                    if ( this.data.autoplay === 'yes' ) {
                        videoElement.setAttribute( 'autoplay', 'autoplay' );
                    } else {
                        videoElement.removeAttribute( 'autoplay' );
                    }

                    if ( this.data.loop === 'yes' ) {
                        videoElement.setAttribute( 'loop', 'loop' );
                    } else {
                        videoElement.removeAttribute( 'loop' );
                    }

                    if ( this.data.allowdownload === 'yes' ) {
                        videoElement.removeAttribute( 'controlslist' );
                    } else {
                        videoElement.setAttribute( 'controlslist', 'nodownload' );
                    }

                    if ( this.data.advisorytitle ) {
                        videoElement.setAttribute( 'title', this.data.advisorytitle );
                    } else {
                        videoElement.removeAttribute( 'title' );
                    }

                    if (this.data.controls) {
                        this.element.getChild(0).setAttribute('controls', 'controls');
                    } else {
                        this.element.getChild(0).removeAttribute('controls');
                    }

                    if (this.data.poster) {
                        this.element.getChild(0).setAttribute('controls', this.data.poster);
                    } else {
                        this.element.getChild(0).removeAttribute('poster');
                    }

                    if (this.data.caption) {
                        this.element.findOne('figcaption').setText(this.data.caption)
                    } else {
                        this.element.findOne('figcaption').setText('')
                    }
                }
            }
        } );

        if ( editor.contextMenu ) {
            editor.addMenuGroup( 'html5videoGroup' );
            editor.addMenuItem( 'html5videoPropertiesItem', {
                label: editor.lang.html5video.videoProperties,
                icon: 'html5video',
                command: 'html5video',
                group: 'html5videoGroup'
            });

            editor.contextMenu.addListener( function( element ) {
                if ( element &&
                     element.getChild( 0 ) &&
                     element.getChild( 0 ).hasClass &&
                     element.getChild( 0 ).hasClass( 'ckeditor-html5-video' ) ) {
                    return { html5videoPropertiesItem: CKEDITOR.TRISTATE_OFF };
                }
            });
        }

        CKEDITOR.dialog.add( 'html5video', this.path + 'dialogs/html5video.js' );
    }
} );
