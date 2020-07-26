var app = angular.module("app", ["ngAnimate", "ui.bootstrap", "kendo.directives","ng-toggle.btn"]);
app.service("cacheManager", function () {
	return {
		setItem: function (key, value) {
			sessionStorage.setItem(key, value);
		},
		getItem: function (key) {
			return sessionStorage.getItem(key);
		},
		removeItem: function (key) {
			sessionStorage.removeItem(key);
		}
	}
});
//app.factory("appEnums", function () {
//	var appEnums = {};
//	appEnums
//	return appEnums;
//});
app.directive('checkBox', [function () {
	var scope = {
		onchange: "&?",
		textClick: "&?",
		checkClick: "&?",
		ngDisabled: "=?",
		ngModel: "=",
		userInput: "=?"
	};

	function link(iScope, iElem, iAttrs, controller) {
		var initState = true;

		$(iElem).css({ 'user-select': 'none' });

		function triggerOnChange() {
			if (typeof (iScope.onchange) != "undefined") {
				var swHandler = $(iElem);
				var dataItem = undefined;
				if (swHandler.parents('.k-treelist').length > 0)
					dataItem = swHandler.parents('.k-treelist').first().data('kendoTreeList').dataItem($(iElem).closest("tr"));
				else if (swHandler.parents('[kendo-grid]').length > 0)
					dataItem = swHandler.parents('[kendo-grid]').first().data('kendoGrid').dataItem($(iElem).closest("tr"));

				var eventParams = {
					target: iElem[0],
					state: iScope.ngModel,
					data: dataItem,
				};
				iScope.onchange({ event: eventParams });
			}
		}

		iScope.checkboxClicked = function () {
			if (!($.trim(iScope.userInput) != "" && !iScope.userInput))
				iScope.ngModel = !iScope.ngModel;

			if (typeof (iScope.checkClick) != "undefined") {
				var swHandler = $(iElem);
				var dataItem = undefined;
				if (swHandler.parents('.k-treelist').length > 0)
					dataItem = swHandler.parents('.k-treelist').first().data('kendoTreeList').dataItem($(iElem).closest("tr"));
				else if (swHandler.parents('[kendo-grid]').length > 0)
					dataItem = swHandler.parents('[kendo-grid]').first().data('kendoGrid').dataItem($(iElem).closest("tr"));

				var eventParams = {
					target: iElem[0],
					state: iScope.ngModel,
					data: dataItem,
				};
				iScope.checkClick({ event: eventParams });
			}
		};

		iScope.textClicked = function () {
			if (typeof (iScope.textClick) != "undefined") {
				var swHandler = $(iElem);
				var dataItem = undefined;
				if (swHandler.parents('.k-treelist').length > 0)
					dataItem = swHandler.parents('.k-treelist').first().data('kendoTreeList').dataItem($(iElem).closest("tr"));
				else if (swHandler.parents('[kendo-grid]').length > 0)
					dataItem = swHandler.parents('[kendo-grid]').first().data('kendoGrid').dataItem($(iElem).closest("tr"));

				var eventParams = {
					target: iElem[0],
					state: iScope.ngModel,
					data: dataItem,
				};
				iScope.textClick({ event: eventParams });
			}
		}

		iScope.$watch(function () { return controller.$modelValue; }, function (newValue, oldValue) {
			if ((newValue != oldValue) || initState) {
				initState = false;
			}
			if (newValue != oldValue) {
				triggerOnChange();
			}
		});
	};

	function template(elem, attr) {
		return '<span class="chk-box" ng-class="{\'has-text-click\': textClick}">' +
			'<i class="fa" ng-class="{\'fa-square-o\': !ngModel, \'fa-check-square\': ngModel}" ng-click="checkboxClicked()"></i>' +
			'<span ng-click="textClicked()" ng-style="{\'cursor\': (textClick ? \'pointer\' : \'default\')}">' + $(elem).html() + '</span>' +
			'</span>';
	}

	return {
		restrict: 'E',
		require: 'ngModel',
		scope: scope,
		link: link,
		template: template
	}
}]);
app.directive('yesNo', [function () {
	
	var scope = {
		initValue: "=?",
		onchange: "&?",
		yes: "=?",
		no: "=?",
		ngDisabled: "=?"
	};

	function link(iScope, iElem, iAttrs, controller) {
		debugger;
		var initState = true, offColor = '#717171', onColor = 'dodgerblue', defaultYesText = "بله", defaultNoText = "خیر";
		if (iAttrs.themeColor) onColor = iAttrs.themeColor;
		$(iElem).css({ 'user-select': 'none' });
		var useBoolean = false;
		for (var attrKey in iAttrs) {
			if (attrKey == "boolean") {
				useBoolean = true;
				break;
			}
		}

		iScope.yes = angular.isDefined(iScope.yes) ? iScope.yes : defaultYesText;
		iScope.no = angular.isDefined(iScope.no) ? iScope.no : defaultNoText;

		function setState(isInit) {
			if (controller.$modelValue == offValue())
				setOffState(isInit);
			else if (controller.$modelValue == onValue())
				setOnState(isInit);
		}

		function setOnState(isInit) {
			iElem.find("[noRadio] > i").removeClass("fa-dot-circle-o").addClass("fa-circle-o").css('color', offColor);
			iElem.find("[yesRadio] > i").removeClass("fa-circle-o").addClass("fa-dot-circle-o").css('color', onColor);
		}

		function setOffState(isInit) {
			iElem.find("[yesRadio] > i").removeClass("fa-dot-circle-o").addClass("fa-circle-o").css('color', offColor);
			iElem.find("[noRadio] > i").removeClass("fa-circle-o").addClass("fa-dot-circle-o").css('color', onColor);
		}

		function triggerOnChange() {
			if (typeof (iScope.onchange) != "undefined") {
				var swHandler = $(iElem).find('.switcher-handle'), dataItem = undefined;
				if (swHandler.parents('.k-treelist').length > 0)
					dataItem = swHandler.parents('.k-treelist').first().data('kendoTreeList').dataItem($(iElem).closest("tr"));
				else if (swHandler.parents('[kendo-grid]').length > 0)
					dataItem = swHandler.parents('[kendo-grid]').first().data('kendoGrid').dataItem($(iElem).closest("tr"));

				var eventParams = {
					target: iElem[0],
					state: controller.$modelValue,
					data: dataItem,
					onValue: onValue(),
					offValue: offValue(),
					isOn: false
				};
				eventParams.isOn = eventParams.state == eventParams.onValue;
				iScope.onchange({ event: eventParams });
			}
		}

		var onValue = function () {
			return useBoolean ? true : 1;
		}
		var offValue = function () {
			return useBoolean ? false : 0;
		}

		iScope.yesRadioClicked = function (e) {
			if (iScope.ngDisabled) return;
			controller.$setViewValue(onValue());
		}

		iScope.noRadioClicked = function (e) {
			if (iScope.ngDisabled) return;
			controller.$setViewValue(offValue());
		}

		iScope.$watch(function () { return controller.$modelValue; }, function (newValue, oldValue) {
			var initStateTemp = initState;
			if ((newValue != oldValue) || initState) {
				initState = false;
				setState(initStateTemp);
			}
			if (newValue != oldValue) {
				triggerOnChange();
			}
		});
	};

	function template(elem, attr) {
		$(elem).addClass('rdo').css({ "display": "block", "min-height": "34px" });
		return '<span yesRadio ng-click="yesRadioClicked($event)"><i class="fa fa-circle-o" style="cursor:pointer"></i></span>' +
			'<label yesLable ng-click="yesRadioClicked($event)" ng-bind="yes" style="cursor:pointer"></label>' +
			'<span noRadio ng-click="noRadioClicked($event)"><i class="fa fa-circle-o" style="cursor:pointer"></i></span>' +
			'<label noLabel ng-click="noRadioClicked($event)" ng-bind="no" style="cursor:pointer"></label>';
	}

	return {
		restrict: 'E',
		require: 'ngModel',
		scope: scope,
		link: link,
		template: template
	}
}]);
app.directive('radioGroup', [function () {
	return {
		restrict: 'E',
		scope: {
			ngModel: "=?",
			options: "=",
			ngDisabled: "=?"
		},
		controller: ["$scope", "$element", "$attrs", function ($scope, $elem, $attrs) {
			$scope.radioClicked = function (e) {
				if ($scope.ngDisabled) return;
				$scope.ngModel = $(e.target).attr("data-item-value");
			};
			$scope.radioLabelClicked = function (e) {
				if ($scope.ngDisabled) return;
				var dataItemValue = $(e.target).parent().find("[radio-box]").attr("data-item-value");
				$scope.ngModel = dataItemValue;
			};
			var triggerOnChange = function () {
				if (typeof ($scope.options.change) == "function") {
					var dataItem = undefined;
					if ($($elem).parents('.k-treelist').length > 0) {
						dataItem = $($elem).parents('.k-treelist').first().data('kendoTreeList').dataItem($($elem).closest("tr"));
					}
					else if ($($elem).parents('[kendo-grid]').length > 0) {
						dataItem = $($elem).parents('[kendo-grid]').first().data('kendoGrid').dataItem($($elem).closest("tr"));
					}
					var eventParams = {
						target: $elem[0],
						value: $scope.ngModel,
						data: dataItem,
					};
					$scope.options.change(eventParams);
				}
			}

			$scope.$watch('ngModel', function (newValue, oldValue) {
				if (newValue != oldValue) {
					triggerOnChange();
				}
			});
		}],
		template: function (elem, attr) {
			$(elem).addClass('rdo');
			return '<span item-wrapper ng-repeat="rdoItem in options.items"><span><i radio-box class="fa" ng-class="{\'fa-circle-o\':(ngModel != rdoItem.value), \'fa-dot-circle-o\':(ngModel == rdoItem.value)}" data-item-value="{{rdoItem.value}}" data-item-text="{{rdoItem.text}}" ng-click="radioClicked($event)" ng-style="{\'color\':(ngModel == rdoItem.value ? (options.onColor ? options.onColor : \'dodgerblue\') : (options.offColor ? options.offColor : \'#717171\'))}"></i></span><label radio-label ng-bind="rdoItem.text" ng-click="radioLabelClicked($event)"></label></span>';
		}
	}
}]);
app.factory("helper", function (cacheManager) {
	var helper = {};
	helper.getFile = function (file) {
		var fileReader = new FileReader(file);
		
	}
	helper.decodeJwt = function (token) {
		var base64Url = token.split('.')[1];
		var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
		var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
			return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
		}).join(''));

		return JSON.parse(jsonPayload);
	}
	function translateFilterToWhere(data) {
		for (var key in data)
		{

		}
	}
	helper.translateToFilterParameter = function (data) {
		return {
			skip: data.skip,
			take: data.take,
			where: data.filter ? {
				Operands: data.filter.filters ? data.filter.filters.map(function (item) {
					return {
						field: item.field,
						value: item.value,
						FilterType: getWhereFilterOperator(item.operator),
					}
				}) : null,
				OperatorType:data.filter.logic
			} : null,
		}
	}
	function getWhereFilterOperator (operator) {
		switch (operator) {
			case "eq":
				return "Equal";
			case "neq":
				return "NotEqual";
			case "contains":
				return "contains";
			default:
				return "none";
			}
	} 
	helper.enums = {
		get UserTypes() {
			return {
				1: {
					text: "مدیر سیستم",
					value: 1
				},
				2: {
					text: "کاربر",
					value: 2
				},
				1000: {
					text: "برنامه نویس",
					value: 1000
				}
			}
		},
		
	}
	return helper;
});
app.service("messageService", function () {

	toastr.options = {
		"closeButton": false,
		"debug": false,
		"newestOnTop": false,
		"progressBar": true,
		"positionClass": "toast-bottom-right",
		"preventDuplicates": false,
		"onclick": null,
		"showDuration": "300",
		"hideDuration": "1000",
		"timeOut": "5000",
		"extendedTimeOut": "1000",
		"showEasing": "swing",
		"hideEasing": "linear",
		"showMethod": "fadeIn",
		"hideMethod": "fadeOut"
	}

	function showMessage(text, title, messageType) {
		toastr[messageType](text, "املاک معمار")
	}

	this.success = function (text, title) {
		showMessage(text, title, "success");
		//alert(text);
	}
	this.error = function (text, title) {
		showMessage(text, title, "error");
		//alert(text);
	}

	this.warn = function (text, title) {
		showMessage(text, title, "warning");
		//alert(text);
	}

	this.info = function (text, title) {
		showMessage(text, title, "info");
	}
})
app.service("dataService", function (messageService, cacheManager) {
	var baseAddress = "/api/";
	var successCallback = [];
	var failCallback;
	var requestHandler = {
		then: function (callback) {
			successCallback = callback;
		},
		fail: function (callback) {
			failCallback = callback;
		}
	}
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
	var httpRequest = function (url, data, method) {
		return $.ajax({
			url: baseAddress + url,
			//type: "json",
			method: method,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			beforeSend: function (xhr) {
				var token = cacheManager.getItem("token");
				//if (!token) document.location.href("/login");
				xhr.setRequestHeader("authorization", "Bearer " + token);
			},
			data: data ? JSON.stringify(data) : null,
		})
	}

	return {
		post: function (url, data, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url, data, "POST").success(function (response, x, jQxhr) {
				var result = processResponse(response, fullResponse);
				if (typeof result!=="undefined")
					deffer.resolve(result);
			})
			return deffer;
		},
		insertEntity: function (url, entity, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url, entity, "POST").success(function (response, x, jQxhr) {
				var result = processResponse(response, fullResponse);
				if (typeof result !== "undefined")
					deffer.resolve(result);
			})
			return deffer;
		},
		updateEntity: function (url, entity, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url, entity, "PUT").success(function (response, x, jQxhr) {
				var result = processResponse(response, fullResponse);
				if (typeof result !== "undefined")
					deffer.resolve(result);
			})
			return deffer;
		},
		deleteEntity: function (url, id, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url + "/" + id,null, "DELETE").success(function (response, x, jQxhr) {
				var result = processResponse(response, fullResponse);
				if (typeof result !== "undefined")
					deffer.resolve(result);
			})
			return deffer;
		},
		getAll: function (url, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url, "GET").then(function (response) {
				var result = processResponse(response, fullResponse);
				if (typeof result !== "undefined")
					deffer.resolve(result);
			})
			//return this.post(url, parameters, fullResponse)
		},
		filterBy: function (url, filterParameter, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			httpRequest(url, filterParameter, "POST").success(function (response, x, jQxhr) {
				var result = processResponse(response, fullResponse);
				if (typeof result !== "undefined")
					deffer.resolve(result);
			})
			return deffer;
		}
	}
});
app.service("securityManager", function (dataService, cacheManager, helper) {
	return {
		login: function (username, password) {
			dataService.post("security/login", {
				userName: username, password: password
			})
				.then(function (result) {
					if (result) {
						debugger;
						var token = result;
						cacheManager.setItem("token", token);
						var user = helper.decodeJwt(token);
						document.location.href = "/Profile";
					}

				});
		},
		get currentUser() {
			var token = cacheManager.getItem("token", token);
			if (token) {
				return helper.decodeJwt(token);
			}
		}
	}
});

