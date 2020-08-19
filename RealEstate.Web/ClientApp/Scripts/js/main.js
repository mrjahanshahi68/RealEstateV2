/*  ---------------------------------------------------
    Template Name: Aler
    Description:  Aler property HTML Template
    Author: Colorlib
    Author URI: https://colorlib.com
    Version: 1.0
    Created: Colorlib
---------------------------------------------------------  */

'use strict';

var dataService = (function () {
	var postDataAndFiles = function (url, parameters, dataFileInfos) {
		var _then = null;
		var promise = (function () {
			return {
				then: function (fn) {
					_then = fn;
				},
			};
		})();

		var ajaxForm = new FormData();
		ajaxForm.append("Parameters", JSON.stringify(parameters));

		var fileKeys = [];
		$(dataFileInfos).each(function (i) {
			var fileKey = "File_" + i;
			ajaxForm.append(fileKey, this.file);
			ajaxForm.append("Data" + fileKey, JSON.stringify(this.data));
			fileKeys.push(fileKey);
		});

		ajaxForm.append("FileKeys", JSON.stringify(fileKeys));

		$.ajax({
			url: url,
			type: "POST",
			dataType: "application/json",
			processData: false,
			contentType: false,
			data: ajaxForm,
			complete: function (jqXHR, statuxText) {
				if (jqXHR.status == 200) {
					if (typeof (_then) == "function")
						_then(JSON.parse(jqXHR.responseText));
				}
			}
		});

		return promise;
	};

	return {
		postDataAndFiles: postDataAndFiles,
	}
})();

var processResponse = function (result, fullResponse) {
	fullResponse = fullResponse ? fullResponse : false;
	if (result["ResultCode"]) {
		if (result["ResultCode"] == 1) {
			if (fullResponse) return result
			else return result["Data"];
		}
		else if (result["ResultCode"] == 2) {
			var failures = $("<ul></ul>")
			for (var i = 0; i < result.Messages.length; i++) {
				failures.append('<li>' + result.Messages[i] + '</li>');
			}
			messageService.error(failures);
			if (failCallback) failCallback(result);
		}
		else if (result["ResultCode"] == 3) {
			messageService.error("عملیات با شکست مواجه شد");
			if (failCallback) failCallback(result);
		}
		else if (result["ResultCode"] == 4 || result["ResultCode"] == 5) {
			document.location.href = "/";
			if (failCallback) failCallback(result);
		}
	}

};

(function ($) {

    /*------------------
        Preloader
    --------------------*/
	$(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Property filter
        --------------------*/
        $('.property-controls li').on('click', function () {
            $('.property-controls li').removeClass('active');
            $(this).addClass('active');
        });
        //if ($('.property-filter').length > 0) {
        //    var containerEl = document.querySelector('.property-filter');
        //    var mixer = mixitup(containerEl);
        //}
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    //Canvas Menu
    $(".canvas-open").on('click', function () {
        $(".offcanvas-menu-wrapper").addClass("show-offcanvas-menu-wrapper");
        $(".offcanvas-menu-overlay").addClass("active");
    });

    $(".canvas-close, .offcanvas-menu-overlay").on('click', function () {
        $(".offcanvas-menu-wrapper").removeClass("show-offcanvas-menu-wrapper");
        $(".offcanvas-menu-overlay").removeClass("active");
    });

    /*------------------
		Navigation
	--------------------*/
    $(".nav-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Carousel Slider
    --------------------*/
    var hero_s = $(".hs-slider");
    hero_s.owlCarousel({
        loop: true,
        margin: 20,
        nav: true,
        items: 1,
        dots: false,
        navText: ['<i class="arrow_left"></i>', '<i class="arrow_right"></i>'],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*------------------
        Team Slider
    --------------------*/
    $(".fp-slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        navText: ['<i class="arrow_left"></i>', '<i class="arrow_right"></i>'],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*------------------
        Testimonial Slider
    --------------------*/
    $(".testimonial-slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 2,
        dots: false,
        nav: true,
        navText: ['<i class="arrow_left"></i>', '<i class="arrow_right"></i>'],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        responsive: {
            0: {
                items: 1
            },
            768: {
                items: 2
            }
        }
    });

    /*------------------
        Logo Slider
    --------------------*/
    $(".lc-slider").owlCarousel({
        loop: true,
        margin: 115,
        items: 6,
        dots: false,
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        responsive: {
            0: {
                items: 2
            },
            480: {
                items: 3
            },
            768: {
                items: 4
            },
            992: {
                items: 5
            },
            1200: {
                items: 6
            }
        }
    });

    /*------------------------
        Property pic slider
    -------------------------*/
    $(".property-pic-slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ['<i class="arrow_left"></i>', '<i class="arrow_right"></i>'],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*------------------------
        Sidebar Feature slider
    -------------------------*/
    $(".sf-slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: true,
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*------------------------
        Agent slider
    -------------------------*/
    $(".as-slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ['<i class="arrow_left"></i>', '<i class="arrow_right"></i>'],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*------------------
        Video Popup
    --------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe'
    });

    /*------------------
        Nice Select
    --------------------*/
    //$('select').niceSelect();

	$('#drp-state').on("change", function () {
		var value = $(this).val();
		var stateCities = cities.filter(function (item) {
			if (item.ParentId == value)
				return item;
		})
		$('#drp-city > option').each(function (i) {
			if (i != 0) $(this).remove();
		})
		
		$('#drp-region > option').each(function (i) {
			if (i != 0) $(this).remove();
		})
		for (var i = 0; i < stateCities.length; i++) {
			$('#drp-city').append(`<option value="${stateCities[i].ID}"> 
                                       ${stateCities[i].Name} 
                                  </option>`); 
		}
		//$('#drp-city').niceSelect('update');
		//$('#drp-region').niceSelect('update');
	});
	$('#drp-city').on("change", function () {
		var value = $(this).val();
		var cityRegions = regions.filter(function (item) {
			if (item.ParentId == value)
				return item;
		})
		$('#drp-region > option').each(function (i) {
			if (i != 0) $(this).remove();
		})


		for (var i = 0; i < cityRegions.length; i++) {
			$('#drp-region').append(`<option value="${cityRegions[i].ID}"> 
                                       ${cityRegions[i].Name} 
                                  </option>`);
		}
		//$('#drp-region').niceSelect('update');
	});
    /*-------------------
		Radio Btn
	--------------------- */
    $(".cb-item label").on('click', function () {
		$(".cb-item label").removeClass('active');
		document.location
        $(this).addClass('active');
    });

    /*-------------------
		Range Wrap
	--------------------- */

    //room size
    $("#roomsize-range").slider({
        range: true,
        min: 0,
        max: 2100,
        step: 300,
        values: [300, 1200],
        slide: function (event, ui) {
            $("#roomsizeRange").val("[" + ui.values[0] + "-" + ui.values[1] + "]" + "مترمربع");
        }
    });
    $("#roomsizeRange").val("[" + $("#roomsize-range").slider("values", 0) + "-" + $("#roomsize-range").slider("values", 1) + "]" + "مترمربع");

    //price range
    $("#price-range").slider({
        range: true,
        min: 0,
        max: 1500,
        step: 100,
        values: [100, 1000],
        slide: function (event, ui) {
            $("#priceRange").val(" ریال" + "[ " + ui.values[0] + " - " + ui.values[1] + " ]");
        }
    });
    $("#priceRange").val(" ریال" + "[ " + $("#price-range").slider("values", 0) + " - " + $("#price-range").slider("values", 1) + " ]" );

     //Text editor
    $('.texteditor-content').richText();

    $('.texteditor-switch').on('click', function () {
        if(!$(this).hasClass('active')) {
            $(".richText-btn[data-command='toggleCode']").click();
        }
    });

     //Drag Upload
    $('.feature-image-content').imageUploader();

	$('#drp-type').on("change", function () {
		if ($(this).val() == 1) {
			$('#sale-container').addClass("d-none");
			$('#mortgage-container').removeClass("d-none");
			$('#rental-container').removeClass("d-none");
		}
		else if ($(this).val() == 2 || $(this).val() == 3) {
			$('#sale-container').removeClass("d-none");
			$('#mortgage-container').addClass("d-none");
			$('#rental-container').addClass("d-none");
		}
		else {
			$('#sale-container').addClass("d-none");
			$('#mortgage-container').addClass("d-none");
			$('#rental-container').addClass("d-none");
		}
	});

	//$('[numeral]').on("change", function (e) {
	//	$(this).val(numeral($(this).val()).format("0,0"));
	//});
	$('[numeral]').on("keyup", function () {
		var myNumeral = numeral(this.value)
		if (myNumeral.value()) {
			this.value = numeral(myNumeral.value()).format("0,0");
		}
	});

	function getFilterParameters() {
		var state;
		var city;
		var region;
		var fullText;;
		var isAdvanced = false;
		var salePrice = {
			minValue: null,
			maxValue: null,
		};
		var mortgagePrice = {
			minValue: null,
			maxValue: null,
		};
		var rentalPrice = {
			minValue: null,
			maxValue: null,
		};
		var landingArea = {
			minValue: null,
			maxValue: null,
		}
		var buildingArea = {
			minValue: null,
			maxValue: null,
		}
		if ($('#collapseOne').hasClass("show")) {
			isAdvanced = true;
		}
		else {
			isAdvanced = false;
		}

		if ($.trim($('#seach-input').val()) != "") {
			fullText = $('#search-input').val();
		}
		if ($.trim($('#drp-state').val()) != "") {
			state = states.filter(function (item) {
				if (item.ID == $('#drp-state').val())
					return item;
			})[0].Name;
		}
		if ($.trim($('#drp-city').val()) != "") {
			city = cities.filter(function (item) {
				if (item.ID == $('#drp-city').val())
					return item;
			})[0].Name;
		}
		if ($.trim($('#drp-region').val()) != "") {
			region = regions.filter(function (item) {
				if (item.ID == $('#drp-region').val())
					return item;
			})[0].Name;
		}
		var regionName = $.trim(state) + " " + $.trim(city) + " " + $.trim(region);

		var type = $('#drp-type').val();;
		if (isAdvanced) {
			if ($.trim($('#drp-type').val()) != "") {
				var saleMinValue = numeral($('#sale-min-price').val()).value();
				var saleMaxValue = numeral($('#sale-max-price').val()).value();
				var mortgageMinValue = numeral($('#mortgage-min-price').val()).value();
				var morgageMaxValue = numeral($('#mortgage-max-price').val()).value();
				var rentalMinValue = numeral($('#rental-min-price').val()).value();
				var rentalMaxValue = numeral($('#rental-max-price').val()).value();

				if (type == 1) {

					mortgagePrice.minValue = mortgageMinValue;
					mortgagePrice.maxValue = morgageMaxValue;
					if (!(mortgagePrice.minValue || mortgagePrice.maxValue)) {
						mortgagePrice = null;
					}

					rentalPrice.minValue = rentalMinValue;
					rentalPrice.maxValue = rentalMaxValue;
					if (!(rentalPrice.minValue || rentalPrice.maxValue)) {
						rentalPrice = null;
					}
				}
				else if (type == 2 || type == 3) {
					salePrice.minValue = saleMinValue;
					salePrice.maxValue = saleMaxValue;
					if (!(salePrice.minValue || salePrice.maxValue)) {
						salePrice = null;
					}
				}
			}
			landingArea.minValue = numeral($('#landing-min-area').val()).value();
			landingArea.maxValue = numeral($('#landing-max-area').val()).value();
			if (!(landingArea.minValue || landingArea.maxValue)) {
				landingArea = null;
			}
			buildingArea.minValue = numeral($('#building-min-area').val()).value();
			buildingArea.maxValue = numeral($('#building-max-area').val()).value();
			if (!(buildingArea.minValue || buildingArea.maxValue)) {
				buildingArea = null;
			}

		}

		var propertyType;
		if ($.trim($('#drp-propertytype').val()) != "") {
			propertyType = $('#drp-propertytype').val();
		}
		var bedroom;
		if ($.trim($('#drp-bedroom').val()) != "") {
			bedroom = $('#drp-bedroom').val();
		}

		var filter = {
			isAdvanced: isAdvanced,
			regionName: regionName,
			propertyType: propertyType,
			bedroom: bedroom,
			type: type,
			fullText: fullText,
			salePrice: salePrice,
			mortgagePrice: mortgagePrice,
			rentalPrice: rentalPrice,
			landingArea: landingArea,
			buildingArea: buildingArea,
		};
		return filter;
	}
	var pageOptions = {
		page: 1,
		take: 12,
		reset: function () {
			this.page = 1;
			this.take = 12;
		}
	}
	function showLoading() {
		$(".loader").css("opacity","1");
		$("#preloder").css("opacity", "1")
		$("#preloder").css("display", "block")
		$(".loader").css("display", "block")
		$('#preloder').css("background","#000000bd")
	}
	function hideLoading() {
		$(".loader").fadeOut();
		$("#preloder").delay(200).fadeOut("slow");
	}
	function generatePropertySearch(data) {
		var renderHtml = "";
		for (var i = 0; i < data.length; i++) {
			var item = data[i];
			var curHtml;
			if (item.Type == 1) {
				curHtml = $('#rent-property-card-template').html();
			}
			else {
				curHtml = $('#sale-property-card-template').html();;
			}
			curHtml = curHtml.replace("{CoverImagePath}", item.CoverImagePath);
			curHtml = curHtml.replace("{CoverImagePath}", item.CoverImagePath);
			curHtml = curHtml.replace("{PropertyTypeName}", item.PropertyTypeName);
			//curHtml = curHtml.replace("{TransactionTypeName}", item.TransactionTypeName);
			if (item.Type == 1) {
				curHtml = curHtml.replace("{MortgagePrice}", numeral(item.MortgagePrice).format("0,0"));
				curHtml = curHtml.replace("{RentalPrice}", numeral(item.RentalPrice).format("0,0"));
			}
			else {
				curHtml = curHtml.replace("{SalePrice}", numeral(item.SalePrice).format("0,0"));
			}
			curHtml = curHtml.replace("{Title}", item.Title);
			curHtml = curHtml.replace("{UrlTitle}", "property/" + item.HashKey + "/" + item.UrlTitle);
			curHtml = curHtml.replace("{RegionName}", item.RegionName);
			if (item.BuildingArea) {
				curHtml = curHtml.replace("{BuildingArea}", item.BuildingArea);
			}
			else {
				curHtml = curHtml.replace("{BuildingArea}", "نامشخص");
			}
			if (item.LandingArea) {
				curHtml = curHtml.replace("{LandingArea}", item.LandingArea);
			}
			else {
				curHtml = curHtml.replace("{LandingArea}", "نامشخص");
			}
			if (item.BedRoom) {
				curHtml = curHtml.replace("{BedRoom}", item.BedRoom);
			}
			else {
				curHtml = curHtml.replace("{BedRoom}", "فاقد اتاق");
			}
			curHtml = curHtml.replace("{PropertyCode}", item.PropertyCode);
			renderHtml += curHtml;

		}
		return renderHtml;
	}
	$('#btn-search').click(function () {
		showLoading();
		pageOptions.reset();
		var filter = getFilterParameters();
		$.ajax({
			url: "home/search",
			method: "POST",
			contentType: "application/json",
			dataType: "json",
			data: JSON.stringify({ filter: filter, page: pageOptions.page, take: pageOptions.take }),
		}).done(function (response) {
			$('#btn-loadmore').css("display", "inline-block");
			$('.property-filter').html('')
			if (response && response.length > 0) {
				var renderHtml = generatePropertySearch(response);
				$('.property-filter').html('');
				$('.property-filter').html(renderHtml);
			}
			else {
				$('.property-filter').html('<div class="col-md-12">نتیحه ای یافت نشد</div>')
			}
			$('.property-filter-title > h3').html("نتیجه جستجو");
			}).always(function () {
				hideLoading();
			})

	});
	
	$('#btn-loadmore').on("click", function () {
		showLoading();
		pageOptions.page++;
		var filter = getFilterParameters();
		$.ajax({
			url: "home/search",
			method: "POST",
			contentType: "application/json",
			dataType: "json",
			data: JSON.stringify({ filter: filter, page: pageOptions.page, take: pageOptions.take }),
		}).done(function (response) {
			if (response && response.length>0) {
				var renderHtml = generatePropertySearch(response);
				$('.property-filter').append(renderHtml);
			}
			else {
				$('#btn-loadmore').css("display", "none");
			}
			}).always(function () {
				hideLoading();
			})
	});

})(jQuery);

