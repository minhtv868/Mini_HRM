(function($){
    /**
     * 최대 글자 수, 현재 글자 수 표시
     * @param {Object} options 옵션
     * @return {Object} jQuery Object this;
     */
    $.fn.maxlength = function (options){
        var settings = $.extend(
            {},
            $.fn.maxlength.defaults,
            options
        );

        var counterElm = '<span class="text-limit">' +
            '<span data-val-length="target"></span>' +
            '<span data-val-length="max"></span>' + 
            '</span>';

        return this.each(function () {
            var $wrap = $(this);
            var $val = $wrap.find('[maxlength]');
            var $valName = settings.valName;
            if ($val.length == 0) {
                $val = $wrap.find('[data-val-maxlength-max]');
                $valName = '[data-val-maxlength-max]';
            }
            var $parent = $wrap.find(settings.parent);
            var maxLength = $val.attr('maxlength') || ($val.attr('data-val-maxlength-max') || 'Vui lòng đặt giá trị maxlength');
            //
            if (!$wrap.find('.text-limit').length) {
                var val = $val.val() || '';

                $parent.length ? $parent.append(counterElm) : $wrap.append(counterElm);
                $wrap.find(settings.target).html(val.length);
                $wrap.find(settings.maxTarget).html('/' + maxLength);

                $wrap.on('keyup', $valName, function (e) {
                    var input = e.currentTarget;
                    var val = input.value;
                    if (val.length > maxLength) input.value = val.substr(0, maxLength);
                     
                    $wrap.find(settings.target).html(input.value.length);
                    //$wrap.find(settings.wordCount).html(input.value.match(/\b\w+\b/g).length);
                });
                $wrap.on('change', $valName, function (e) {
                    var input = e.currentTarget;
                    var val = input.value;
                    if (val.length > maxLength) input.value = val.substr(0, maxLength);
                    $wrap.find(settings.target).html(input.value.length);
                    //$wrap.find(settings.wordCount).html(input.value.match(/\b\w+\b/g).length);
                });
            }
        });
    };

    $.fn.maxlength.defaults = {
        valName: '[maxlength]',
        target: '[data-val-length=target]',
        maxTarget: '[data-val-length=max]',
        parent: '[data-val-length=parent]',
        wordCount: '[data-val-word=parent]',
    };
})(jQuery);
