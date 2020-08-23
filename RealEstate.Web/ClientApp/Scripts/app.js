var routePrefix="/"
Array.prototype.findById = function (field, value) {
	for (var i = 0; i < this.length; i++) {
		var item = this[i];
		if (item[field] == value) {
			return item;
		}
	}
}
Array.prototype.deleteById = function (field, value) {
	for (var i = 0; i < this.length; i++) {
		var item = this[i];
		if (item[field] == value) {
			this.splice(i, 1);
			break;
		}
	}
}
Array.prototype.copy = function () {
	var stringifyArray = JSON.stringify(this);
	return JSON.parse(stringifyArray);
}

var app = angular.module("app", ["ngAnimate", "ui.bootstrap", "kendo.directives", "ng-toggle.btn"]);
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
var DataFileInfo = function (file, data) {
	this.file = file;
	this.data = data;
};

app.directive("switcher", function () {
	return {
		restrict: 'E',
		scope: {
			ngModel: "=?",
			ngChange: "&?",
		},
		controller: function ($scope) {
			$scope.switcherClicked = function () {
				$scope.ngModel = !$scope.ngModel;
			};

			$scope.$watch(function (scope) { return $scope.ngModel; }, function (newValue, oldValue) {
				if (newValue != oldValue) {
					if (typeof ($scope.ngChange) == "function") {
						$scope.ngChange({
							status: $scope.ngModel,
							$event: event,
							sender: event.target,
						});
					}
				}
			});
		},
		link: function ($scope, $elem, $attr) {

		},
		template: '<span class="fa" ng-class="{\'fa-toggle-on\':ngModel, \'fa-toggle-off\':!ngModel}" ng-click="switcherClicked()" style="font-size:30px"></span>',
	};
});
app.factory("helper", function (cacheManager) {
	var helper = {};
	helper.getFile = function (file) {
		var fileReader = new FileReader(file);
	}
	helper.getFileInfo = function (files) {
		debugger;
		var _files = [];;
		if (!files) throw Error("NullReferenceException");
		if (files instanceof Array) {
			_files = files;
		} else {
			_files.push(files);
		}
		var _onProgressCallback = null;
		var _onProgress = function (callback) {
			_onProgressCallback = callback;
			return fileHandler;
		}

		var _onCompleteCallback = null;
		var _onComplete = function (callback) {
			_onCompleteCallback = callback;
			return fileHandler;
		}
		var _onCompleteAllCallback = null;
		var _onCompleteAll = function (callback) {
			_onCompleteAllCallback = callback;
			return fileHandler;
		}

		var _startUpload = function () {
			for (var i = 0; i < attachmenst.length; i++) {
				var _fileReader = attachmenst[i].fileReader;
				_fileReader.onprogress = function (e, b) {
					var uid = e.srcElement.uid;
					var _currentAttachment = attachmenst.findById("key", uid);
					loadedSize += e.loaded;
					_currentAttachment.loaded = e.loaded;
					_currentAttachment.total = e.total;
					_currentAttachment.element = e;
					if (_onProgressCallback) _onProgressCallback(_currentAttachment, e, totalSize)
				}
				_fileReader.onloadend = function (e) {
					var uid = e.srcElement.uid;
					var _currentAttachment = attachmenst.findById("key", uid);
					_currentAttachment.fileReader.base64 = e.target.result;
					_currentAttachment.isLoaded = true;
					if (_onCompleteCallback) _onCompleteCallback(_currentAttachment, e, totalSize)
					if (loadedSize == totalSize) {
						if (_onCompleteAllCallback) _onCompleteAllCallback(attachmenst)
					}
				}
				_fileReader.readAsDataURL(attachmenst[i].file);
			}
			return fileHandler;
		}
		var fileHandler = {

			base64: null,
			start: _startUpload,
			onComplete: _onComplete,
			onProgress: _onProgress,
			onCompleteAll: _onCompleteAll,
		}

		var attachmenst = [];
		var totalSize = 0;
		var loadedSize = 0;
		for (var i = 0; i < _files.length; i++) {
			var file = _files[i];
			if (file) {
				var fileReader = new FileReader();
				totalSize += file.size;
				var uid = helper.uuidv4();
				fileReader.uid = uid;
				attachmenst.push({
					fileName: file.name,
					size: file.size,
					contentType: file.type,
					key: uid,
					file: file,
					element: null,
					fileReader: fileReader,
					isLoaded: false,
					loaded: 0,
					total: 0,
				});
			}
		}

		return fileHandler;
	}	

	helper.decodeJwt = function (token) {
		var base64Url = token.split('.')[1];
		var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
		var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
			return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
		}).join(''));

		return JSON.parse(jsonPayload);
	}
	helper.getParameterByName = function (name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, '\\$&');
		var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
			results = regex.exec(url);
		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, ' '));
	}
	helper.uuidv4 = function () {
		return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
			var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
			return v.toString(16);
		});
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
				return "Contains";
			case "lte":
				return "LessThanOrEqual";
			case "gte":
				return "GreaterThanOrEqual";
			default:
				return "contains";
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
		get CustomerTypes() {
			return {
				Owner: {
					text: "مالک",
					value: 1
				},
				PropertyApplicant: {
					text: "متقاضی",
					value: 2
				}
			}
		},
		get StateLevel() {
			return {
				State: {
					text: "استان",
					value: 0
				},
				City: {
					text: "شهر",
					value: 1
				},
				Region: {
					text: "منطقه",
					value: 2
				}
			}
		},
		get PropertyStatuses() {
			return {
				Submitted: {
					text: "ارسال شده",
					value: 1
				},
				Approved: {
					text: "تایید شده",
					value: 2
				},
				Sold: {
					text: "فروخته شده",
					value: 3
				},
				Leased: {
					text: "اجاره داده شده",
					value: 4
				},
				Rejected: {
					text: "رد شده",
					value: 5
				}
			}
		},
		get TransactionTypes() {
			return {
				Rent: {
					text: "اجاره",
					value: 1
				},
				Sale: {
					text: "فروش",
					value: 2
				},
				PreSel: {
					text: "پیش فروش",
					value: 3
				},
				Exchange: {
					text: "معاوضه",
					value: 4
				}
			}
		},
		get PropertyStatus() {
			return {
				Submitted: {
					text: "ارسال شد",
					value: 1
				},
				Approved: {
					text: "تایید شد",
					value: 2
				},
				Sold: {
					text: "فروخته شد",
					value: 3
				},
				Leased: {
					text: "اجاره رفته",
					value: 4
				},
				Rejected: {
					text: "رد شد",
					value: 5
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
		toastr[messageType](text, "املاک vip")
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
				document.location.href = "/login";
				if (failCallback) failCallback(result);
			}
		}

	};
	var httpRequest = function (url, data, method) {
		return $.ajax({
			url: baseAddress + url,
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
		postDataAndFiles: function (url, parameters, dataFileInfos, fullResponse) {
			if ($.trim(url) != "") {
				if (url.indexOf("/") == 0) url = url.substr(1);
			}
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
			var deffer = $.Deferred();
			deffer.promise(requestHandler);
			var httpRequest = $.ajax({
				url: baseAddress + url,
				type: "POST",
				dataType: "application/json",
				processData: false,
				contentType: false,
				data: ajaxForm,
				beforeSend: function (xhr) {
					var token = cacheManager.getItem("token");
					//if (!token) document.location.href("/login");
					xhr.setRequestHeader("authorization", "Bearer " + token);
				},
				complete: function (jqXHR, statuxText) {
					if (jqXHR.status == 200) {
						var response = JSON.parse(jqXHR.responseText);
						var result = processResponse(response, fullResponse);
						if (typeof result !== "undefined")
							deffer.resolve(result);
					}
				}
			});

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
		},

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
						dataService.post("security/GetCurrentUser").then(function (result) {
							cacheManager.setItem("userInfo", JSON.stringify(result));
							document.location.href = routePrefix + "Profile";
						})
					}

				});
		},
		loggout: function () {
			cacheManager.removeItem("token");
			cacheManager.removeItem("userInfo");
			document.location.href = routePrefix + "login";
		},
		checkAuthenticate: function (callbcack) {
			var req = dataService.post("security/refereshToken");
			if (callbcack && typeof (callbcack) === "function")
				req.then(callbcack);
		},
		get currentUser() {
			var userInfo = cacheManager.getItem("userInfo");
			if (userInfo)
				return JSON.parse(userInfo);
			
		}
	}
});

app.run(function ($rootScope, securityManager) {
	if (securityManager.currentUser) {
		$rootScope.currentUser = securityManager.currentUser;
		$rootScope.fullName = "کاربر " + $rootScope.currentUser.FullName + " خوش آمدید"

		var intVal = setInterval(function () {
			securityManager.checkAuthenticate(function (isAuth) {
				if (!isAuth) {
					clearInterval(intVal);
					document.location.href = routePrefix + "login";
				}
			});
		}, 30000)
	}
	$rootScope.loggout = function () {
		securityManager.loggout();
	}
})

