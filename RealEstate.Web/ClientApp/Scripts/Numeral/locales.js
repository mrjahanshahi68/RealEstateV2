/*! @preserve
 * numeral.js
 * locales : 2.0.6
 * license : MIT
 * http://adamwdraper.github.com/Numeral-js/
 */

(function (global, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['numeral'], factory);
    } else if (typeof module === 'object' && module.exports) {
        factory(require('./numeral'));
    } else {
        factory(global.numeral);
    }
}(this, function (numeral) {

	(function () {
	  numeral.register('locale', 'fa-IR', {
		delimiters: {
		  thousands: ',',
		  decimal: '.'
		},
		abbreviations: {
		  thousand: 'هزار',
		  million: 'میلیون',
		  billion: 'بیلیون',
		  trillion: 'تریلیون'
		},
		ordinal: function (number) {
		  var b = number % 10;
		  return (~~(number % 100 / 10) === 1) ? 'th' :
			(b === 1) ? 'st' :
			  (b === 2) ? 'nd' :
				(b === 3) ? 'rd' : 'th';
		},
		currency: {
		  symbol: 'تومان'
		}
	  });
	})();


