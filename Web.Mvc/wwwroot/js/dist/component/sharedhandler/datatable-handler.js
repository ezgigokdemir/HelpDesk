/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./wwwroot/js/src/component/sharedhandler/datatable-handler.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./wwwroot/js/src/component/sharedhandler/datatable-handler.js":
/*!*********************************************************************!*\
  !*** ./wwwroot/js/src/component/sharedhandler/datatable-handler.js ***!
  \*********************************************************************/
/*! exports provided: tableHandler */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"tableHandler\", function() { return tableHandler; });\nvar tableHandler = {\n  handleResponse: function handleResponse(self, response, pageNumber, pageLength) {\n    self.tableConfig = response.data;\n    self.tableConfig.pages = [];\n    self.tableConfig.currentPage = pageNumber;\n    self.tableConfig.numberOfPages = Math.ceil(self.tableConfig.records * 1.0 / pageLength);\n\n    if (self.tableConfig.numberOfPages <= 5) {\n      for (var i = 1; i <= self.tableConfig.numberOfPages; i++) {\n        self.tableConfig.pages.push({\n          pageNumber: i,\n          pageLength: pageLength,\n          active: pageNumber === i,\n          disabled: false\n        });\n      }\n    } else {\n      if (pageNumber <= 4) {\n        for (var _i = 1; _i <= 5; _i++) {\n          self.tableConfig.pages.push({\n            pageNumber: _i,\n            pageLength: pageLength,\n            active: pageNumber === _i,\n            disabled: false\n          });\n        }\n\n        self.tableConfig.pages.push({\n          disabled: true\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages\n        });\n      } else if (pageNumber + 2 <= self.tableConfig.numberOfPages) {\n        self.tableConfig.pages.push({\n          pageNumber: 1,\n          pageLength: pageLength,\n          active: false,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          disabled: true\n        });\n        self.tableConfig.pages.push({\n          pageNumber: pageNumber - 1,\n          pageLength: pageLength,\n          active: false,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: pageNumber,\n          pageLength: pageLength,\n          active: true,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: pageNumber + 1,\n          pageLength: pageLength,\n          active: false,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          disabled: true\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages,\n          pageLength: pageLength,\n          active: false\n        });\n      } else {\n        self.tableConfig.pages.push({\n          pageNumber: 1,\n          pageLength: pageLength,\n          active: pageNumber === 1,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          disabled: true\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages - 4,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages - 2,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages - 3,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages - 2,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages - 2,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages - 2,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages - 1,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages - 1,\n          disabled: false\n        });\n        self.tableConfig.pages.push({\n          pageNumber: self.tableConfig.numberOfPages,\n          pageLength: pageLength,\n          active: pageNumber === self.tableConfig.numberOfPages,\n          disabled: false\n        });\n      }\n    }\n  }\n};\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi93d3dyb290L2pzL3NyYy9jb21wb25lbnQvc2hhcmVkaGFuZGxlci9kYXRhdGFibGUtaGFuZGxlci5qcy5qcyIsInNvdXJjZXMiOlsid2VicGFjazovLy8uL3d3d3Jvb3QvanMvc3JjL2NvbXBvbmVudC9zaGFyZWRoYW5kbGVyL2RhdGF0YWJsZS1oYW5kbGVyLmpzPzczNjgiXSwic291cmNlc0NvbnRlbnQiOlsidmFyIHRhYmxlSGFuZGxlciA9IHtcbiAgaGFuZGxlUmVzcG9uc2U6IGZ1bmN0aW9uIGhhbmRsZVJlc3BvbnNlKHNlbGYsIHJlc3BvbnNlLCBwYWdlTnVtYmVyLCBwYWdlTGVuZ3RoKSB7XG4gICAgc2VsZi50YWJsZUNvbmZpZyA9IHJlc3BvbnNlLmRhdGE7XG4gICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcyA9IFtdO1xuICAgIHNlbGYudGFibGVDb25maWcuY3VycmVudFBhZ2UgPSBwYWdlTnVtYmVyO1xuICAgIHNlbGYudGFibGVDb25maWcubnVtYmVyT2ZQYWdlcyA9IE1hdGguY2VpbChzZWxmLnRhYmxlQ29uZmlnLnJlY29yZHMgKiAxLjAgLyBwYWdlTGVuZ3RoKTtcblxuICAgIGlmIChzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgPD0gNSkge1xuICAgICAgZm9yICh2YXIgaSA9IDE7IGkgPD0gc2VsZi50YWJsZUNvbmZpZy5udW1iZXJPZlBhZ2VzOyBpKyspIHtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBpLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBpLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgIH1cbiAgICB9IGVsc2Uge1xuICAgICAgaWYgKHBhZ2VOdW1iZXIgPD0gNCkge1xuICAgICAgICBmb3IgKHZhciBfaSA9IDE7IF9pIDw9IDU7IF9pKyspIHtcbiAgICAgICAgICBzZWxmLnRhYmxlQ29uZmlnLnBhZ2VzLnB1c2goe1xuICAgICAgICAgICAgcGFnZU51bWJlcjogX2ksXG4gICAgICAgICAgICBwYWdlTGVuZ3RoOiBwYWdlTGVuZ3RoLFxuICAgICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBfaSxcbiAgICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICAgIH0pO1xuICAgICAgICB9XG5cbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBkaXNhYmxlZDogdHJ1ZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMsXG4gICAgICAgICAgcGFnZUxlbmd0aDogcGFnZUxlbmd0aCxcbiAgICAgICAgICBhY3RpdmU6IHBhZ2VOdW1iZXIgPT09IHNlbGYudGFibGVDb25maWcubnVtYmVyT2ZQYWdlc1xuICAgICAgICB9KTtcbiAgICAgIH0gZWxzZSBpZiAocGFnZU51bWJlciArIDIgPD0gc2VsZi50YWJsZUNvbmZpZy5udW1iZXJPZlBhZ2VzKSB7XG4gICAgICAgIHNlbGYudGFibGVDb25maWcucGFnZXMucHVzaCh7XG4gICAgICAgICAgcGFnZU51bWJlcjogMSxcbiAgICAgICAgICBwYWdlTGVuZ3RoOiBwYWdlTGVuZ3RoLFxuICAgICAgICAgIGFjdGl2ZTogZmFsc2UsXG4gICAgICAgICAgZGlzYWJsZWQ6IGZhbHNlXG4gICAgICAgIH0pO1xuICAgICAgICBzZWxmLnRhYmxlQ29uZmlnLnBhZ2VzLnB1c2goe1xuICAgICAgICAgIGRpc2FibGVkOiB0cnVlXG4gICAgICAgIH0pO1xuICAgICAgICBzZWxmLnRhYmxlQ29uZmlnLnBhZ2VzLnB1c2goe1xuICAgICAgICAgIHBhZ2VOdW1iZXI6IHBhZ2VOdW1iZXIgLSAxLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBmYWxzZSxcbiAgICAgICAgICBkaXNhYmxlZDogZmFsc2VcbiAgICAgICAgfSk7XG4gICAgICAgIHNlbGYudGFibGVDb25maWcucGFnZXMucHVzaCh7XG4gICAgICAgICAgcGFnZU51bWJlcjogcGFnZU51bWJlcixcbiAgICAgICAgICBwYWdlTGVuZ3RoOiBwYWdlTGVuZ3RoLFxuICAgICAgICAgIGFjdGl2ZTogdHJ1ZSxcbiAgICAgICAgICBkaXNhYmxlZDogZmFsc2VcbiAgICAgICAgfSk7XG4gICAgICAgIHNlbGYudGFibGVDb25maWcucGFnZXMucHVzaCh7XG4gICAgICAgICAgcGFnZU51bWJlcjogcGFnZU51bWJlciArIDEsXG4gICAgICAgICAgcGFnZUxlbmd0aDogcGFnZUxlbmd0aCxcbiAgICAgICAgICBhY3RpdmU6IGZhbHNlLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBkaXNhYmxlZDogdHJ1ZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMsXG4gICAgICAgICAgcGFnZUxlbmd0aDogcGFnZUxlbmd0aCxcbiAgICAgICAgICBhY3RpdmU6IGZhbHNlXG4gICAgICAgIH0pO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiAxLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSAxLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBkaXNhYmxlZDogdHJ1ZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSA0LFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAyLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAzLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAyLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAyLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAyLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAxLFxuICAgICAgICAgIHBhZ2VMZW5ndGg6IHBhZ2VMZW5ndGgsXG4gICAgICAgICAgYWN0aXZlOiBwYWdlTnVtYmVyID09PSBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMgLSAxLFxuICAgICAgICAgIGRpc2FibGVkOiBmYWxzZVxuICAgICAgICB9KTtcbiAgICAgICAgc2VsZi50YWJsZUNvbmZpZy5wYWdlcy5wdXNoKHtcbiAgICAgICAgICBwYWdlTnVtYmVyOiBzZWxmLnRhYmxlQ29uZmlnLm51bWJlck9mUGFnZXMsXG4gICAgICAgICAgcGFnZUxlbmd0aDogcGFnZUxlbmd0aCxcbiAgICAgICAgICBhY3RpdmU6IHBhZ2VOdW1iZXIgPT09IHNlbGYudGFibGVDb25maWcubnVtYmVyT2ZQYWdlcyxcbiAgICAgICAgICBkaXNhYmxlZDogZmFsc2VcbiAgICAgICAgfSk7XG4gICAgICB9XG4gICAgfVxuICB9XG59O1xuZXhwb3J0IHsgdGFibGVIYW5kbGVyIH07Il0sIm1hcHBpbmdzIjoiQUFBQTtBQUFBO0FBQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7Iiwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./wwwroot/js/src/component/sharedhandler/datatable-handler.js\n");

/***/ })

/******/ });