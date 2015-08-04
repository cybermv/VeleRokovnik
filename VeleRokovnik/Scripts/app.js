var rokovnikApp = angular.module('VeleRokovnikApp', []);

rokovnikApp.controller('ObvezeController', ['$scope', '$timeout', '$interval', 'ObvezeFactory',
    function ($scope, $timeout, $interval, obvezeFactory) {
        var ctrl = this;

        ctrl.osobaId = $('#osobaId').attr('value');

        ctrl.obveze = obvezeFactory.getObvezeFor(ctrl.osobaId);
        ctrl.vrsteObveze = obvezeFactory.getVrsteObveze();
        ctrl.activeObveza = obvezeFactory.getActiveObveza();

        ctrl.showObveza = function (obvezaId) {
            obvezeFactory.showObveza(obvezaId);
        };

        ctrl.editObveza = function (obvezaId) {
            obvezeFactory.editObveza(obvezaId);
        };

        ctrl.saveObveza = function () {
            obvezeFactory.saveActiveObveza();
        };

        ctrl.newObveza = function () {
            obvezeFactory.newObveza();
        };

        ctrl.deleteObveza = function (obvezaId) {
            obvezeFactory.deleteObveza(obvezaId);
        };

        ctrl.dismissModals = function () {
            $('.modal').modal('hide');
        };

        ctrl.idToVrstaObveze = function (id) {
            var text;
            angular.forEach(ctrl.vrsteObveze, function (item) {
                if (item.Value === id)
                    text = item.Text;
            });
            return text;
        };

        ctrl.shareObveza = function () {
            ctrl.dismissModals();
            $timeout(function () {
                $('#modal-share-obveza').modal('show');
            }, 1000);

        };

        ctrl.danas = new Date();
        ctrl.searchDate = new Date();

        $interval(function () {
            ctrl.danas = new Date();
        }, 10000);
    }
]);

rokovnikApp.factory('ObvezeFactory', ['ObvezeService',
    function (obvezeService) {
        var toReturn = {};

        var osobaId = -1;

        var obveze = [];
        var vrsteObveze = [];
        var activeObveza = {};

        obvezeService.getVrsteObveze().then(
            function (successData) {
                angular.copy(successData, vrsteObveze);
            },
            function (errorData) {
                console.log(errorData);
            });

        toReturn.getObvezeFor = function (oId) {
            osobaId = oId;
            obvezeService.getObvezeFor(osobaId).then(
                function (successData) {
                    angular.copy(successData, obveze);
                },
                function (errorData) {
                    console.log(errorData);
                });

            return obveze;
        };

        toReturn.getVrsteObveze = function () {
            return vrsteObveze;
        };

        toReturn.getActiveObveza = function () {
            return activeObveza;
        };

        toReturn.showObveza = function (obvezaId) {
            obvezeService.getObvezaById(obvezaId).then(
                function (successData) {
                    angular.copy(successData, activeObveza);
                    $('#modal-show-obveza').modal('show');
                },
                function (errorData) {
                    console.log(errorData);
                });

        };

        toReturn.editObveza = function (obvezaId) {
            obvezeService.getObvezaById(obvezaId).then(
                function (successData) {
                    angular.copy(successData, activeObveza);
                    $('#modal-edit-obveza').modal('show');
                },
                function (errorData) {
                    console.log(errorData);
                });
        };

        toReturn.saveActiveObveza = function () {
            var isNew = activeObveza.ObvezaId === null;
            obvezeService.saveObveza(activeObveza).then(
                function (successData) {
                    if (isNew) {
                        obveze.push(successData);
                    } else {
                        angular.forEach(obveze, function (item) {
                            if (item.ObvezaId === successData.ObvezaId) {
                                angular.copy(successData, item);
                            }
                        });
                    }
                    console.log(successData);
                    $('.modal').modal('hide');
                },
                function (errorData) {
                    console.log(errorData);
                });
        };

        toReturn.newObveza = function () {
            obvezeService.newObveza().then(
                function (successData) {
                    angular.copy(successData, activeObveza);
                    activeObveza.OsobaId = osobaId;
                    $('#modal-edit-obveza').modal('show');
                },
                function (errorData) {
                    console.log(errorData);
                });
        };

        toReturn.deleteObveza = function (obvezaId) {
            obvezeService.deleteObveza(obvezaId).then(
                function (successData) {
                    console.log(successData);
                    angular.forEach(obveze, function (item) {
                        if (item.ObvezaId == obvezaId) {
                            obveze.splice(obveze.indexOf(item), 1);
                        }
                    });
                    $('#modal-deleted-obveza').modal('show');
                },
                function (errorData) {
                    console.log(errorData);
                });
        };

        toReturn.idToVrstaObveze = function (id) {
            var text = "";
            angular.forEach(vrsteObveze, function (item) {
                if (item.Value == id) {
                    text = item.Text;
                }
            });
            return text;
        };

        return toReturn;
    }
]);

rokovnikApp.service('ObvezeService', [
    '$http', '$q', function ($http) {
        return {
            getObvezeFor: getObvezeFor,
            getObvezaById: getObvezaById,
            getVrsteObveze: getVrsteObveze,
            saveObveza: saveObveza,
            newObveza: newObveza,
            deleteObveza: deleteObveza
        };

        function getObvezeFor(osobaId) {
            return $http.get('/Obveze/Get', { params: { osobaId: osobaId } }).then(handleSuccess, handleError);
        }

        function getObvezaById(obvezaId) {
            return $http.get('/Obveze/GetById', { params: { obvezaId: obvezaId } }).then(handleSuccess, handleError);
        }

        function getVrsteObveze() {
            return $http.get('/Obveze/GetVrsteObveze').then(handleSuccess, handleError);
        }

        function saveObveza(obveza) {
            console.warn("SAVE: " + JSON.stringify(obveza));
            return $http.post('/Obveze/Save', obveza).then(handleSuccess, handleError);
        }

        function newObveza() {
            return $http.get('/Obveze/New').then(handleSuccess, handleError);
        }

        function deleteObveza(obvezaId) {
            return $http.post('/Obveze/Delete', { obvezaId: obvezaId }).then(handleSuccess, handleError);
        }

        function handleSuccess(successData) {
            /* radi glupog nestandardnog ugradjenog json serialisera
               u asp.net mvc-u, potrebno je ovdje interceptati
               datum i pretvoriti ga u JS objekt s gornjom funkcijom
               mvcDateToJsDate - ova konverzija bi se trebala
               prebaciti na server
            
            if (successData.data.Datum) {
                successData.data.Datum = mvcDateToJsDate(successData.data.Datum);
            }*/

            return successData.data;
        }

        function handleError(errorData) {
            return errorData;
        }
    }
]);

rokovnikApp.directive('datepicker', ['$filter',
    function ($filter) {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelController) {
                ngModelController.$formatters.push(function (data) {
                    return $filter('date')(data, 'dd.MM.yyyy');
                });
            }
        }
    }
]);

rokovnikApp.filter('shorter',
    function () {
        return function (value, length) {
            if (!length) length = 40;
            if (value.length <= length) {
                return value;
            } else {
                var crude = value.substring(0, length);
                var lastSpace = crude.lastIndexOf(' ');
                return crude.substring(0, lastSpace) + '...';
            }
        };
    });

rokovnikApp.filter('vrstaobveze', ['ObvezeFactory',
    function (obvezeFactory) {
        return function (value) {
            return obvezeFactory.idToVrstaObveze(value);
        };
    }
]);

rokovnikApp.filter('bydate', [
    function () {
        return function (values, parameter) {
            if (!parameter) {
                return values;
            }
            var expected = parameter;
            if (!(parameter instanceof Date)) {
                var pieces = parameter.split('.');
                var tmp = pieces[0];
                pieces[0] = pieces[1];
                pieces[1] = tmp;
                expected = new Date(pieces.join('.'));
            }

            var filtered = [];

            angular.forEach(values, function (item) {
                if (new Date(item.Datum).getDate() == expected.getDate()) {
                    filtered.push(item);
                }
            });

            return filtered;
        }
    }
]);