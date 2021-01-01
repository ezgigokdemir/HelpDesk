import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import moment from 'moment';
import '../../../../../node_modules/toastr/build/toastr.css';
import toastr from 'toastr';

toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

Vue.use(VueTheMask);
Vue.use(VeeValidate);

Validator.localize('tr', tr);

var app = new Vue({
    el: '#app',
    components: {
        'message': message
    },
    data: {
        config: {
            messages: [],
            filterType: 'notAssigned'
        },
        demands: {},
        urgencies: urgencies,
        orderType: 0
    },
    methods: {

        getAllDemandsByFilter: function () {
            let self = this;
            axios.get('/demand/getallincomingdemands', {
                params: {
                    orderType: self.orderType,
                    filterType: self.config.filterType
                }
            }).then(function (response) {
                self.demands = response.data;
            })
                .catch(function (error) {
                    console.log(error);
                })
                .then(function () {

                });
        },

        assignDemand: function (demandId) {
            let self = this;
            swal.fire({
                title: 'Talebi almak istediğinize emin misiniz?',
                showCancelButton: true,
                confirmButtonText: 'Atama Yap',
                cancelButtonText: 'İptal',
                confirmButtonColor: '#4e73df',
                cancelButtonColor: '#4e73df'
            }).then((result) => {
                if (result.value) {
                    var demandDto = {};
                    demandDto.Id = demandId;
                    demandDto.IsAccepted = true;
                    axios.post('/demand/assigndemand', demandDto).then(function (response) {
                        toastr.info('Atama başarıyla gerçekleştirildi.');
                        let id = self.demands.findIndex(val => val.Id === demandId);
                        self.demands.splice(id, 1);
                    }).catch(function (error) {
                        console.log(error);
                        toastr.error('İşlem gerçekleştirilemedi!');
                    }).then(function () {

                    });
                }
            })
        },

        setDemandAsSolved: function (demandId) {
            let self = this;
            swal.fire({
                title: 'Talebi çözüldü olarak işaretlemek istiyor musunuz?',
                showCancelButton: true,
                confirmButtonText: 'Çözüldü',
                cancelButtonText: 'İptal',
                confirmButtonColor: '#4e73df',
                cancelButtonColor: '#4e73df'
            }).then((result) => {
                if (result.value) {
                    var demandDto = {};
                    demandDto.Id = demandId;
                    demandDto.IsDissolved = true;
                    axios.post('/demand/setdemandassolved', demandDto).then(function (response) {
                        toastr.info('Kayıt başarıyla eklendi.');
                        let id = self.demands.findIndex(val => val.Id === demandId);
                        self.demands.splice(id, 1);
                    }).catch(function (error) {
                        console.log(error);
                        toastr.error('İşlem gerçekleştirilemedi!');
                    }).then(function () {

                    });
                }
            })
        },

        cancelDemandAssignment: function (demandId) {
            let self = this;
            swal.fire({
                title: 'Talebin atamasını iptal etmek istiyor musunuz?',
                showCancelButton: true,
                confirmButtonText: 'Atama İptal Et',
                cancelButtonText: 'Vazgeç',
                confirmButtonColor: '#4e73df',
                cancelButtonColor: '#4e73df'
            }).then((result) => {
                if (result.value) {
                    var demandDto = {};
                    demandDto.Id = demandId;
                    demandDto.IsAccepted = false;
                    axios.post('/demand/cancelDemandAssignment', demandDto).then(function (response) {
                        toastr.info('Atama başarıyla iptal edildi.');
                        let id = self.demands.findIndex(val => val.Id === demandId);
                        self.demands.splice(id, 1);
                    }).catch(function (error) {
                        console.log(error);
                        toastr.error('İşlem gerçekleştirilemedi!');
                    }).then(function () {

                    });
                }
            })
        },

        setDemandAsCompleted: function (demandId) {
            let self = this;
            swal.fire({
                title: 'Talebin atamasını iptal etmek istiyor musunuz?',
                showCancelButton: true,
                confirmButtonText: 'Tamamlandı',
                cancelButtonText: 'İptal',
                confirmButtonColor: '#4e73df',
                cancelButtonColor: '#4e73df'
            }).then((result) => {
                if (result.value) {
                    var demandDto = {};
                    demandDto.Id = demandId;
                    demandDto.IsCompleted = true;
                    axios.post('/demand/setDemandAsCompleted', demandDto).then(function (response) {
                        toastr.info('İşlem tamamlandı!');
                        let id = self.demands.findIndex(val => val.Id === demandId);
                        self.demands.splice(id, 1);
                    }).catch(function (error) {
                        console.log(error);
                        toastr.error('İşlem gerçekleştirilemedi!');
                    }).then(function () {

                    });
                }
            })
        }
    },

    created: function () {

    },

    mounted: function () {
        this.$nextTick(this.getAllDemandsByFilter);
    },

    computed: {

    },

    filters: {
        formatDate: function (value) {
            if (!value) return '';
            return moment(String(value)).format('DD/MM/YYYY');
        }
    }
})
