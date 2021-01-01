import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import { tableHandler } from '../sharedhandler/datatable-handler';
import moment from 'moment';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';

toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

Vue.use(VueTheMask);
Vue.use(VeeValidate);

var app = new Vue({
    el: '#app',
    components: {
        'message': message
    },
    data: {
        config: {
            messages: [],
            filterType: 'fullname'
        },
        // users: users,
        // companyId: companyId,
        tableConfig: {},
        selection: {
            selectedClient: {}
        },
    },
    methods: {

        paginate: function (pageNumber, pageLength) {
            var self = this;

            axios.get('/identity/companyuser/getall', {
                params: {
                    pageNumber: pageNumber,
                    pageLength: pageLength,
                    filterType: self.config.filterType,
                    keyword: self.config.keyword,
                    // companyId:self.companyId,
                }
            })
                .then(function (response) {
                    // console.log(response);
                    tableHandler.handleResponse(self, response, pageNumber, pageLength);
                })
                .catch(function (error) {
                    console.log(error);
                })
                .then(function () {

                });
        },

        deleteUser: function (id) {
            console.log("js ici id", id);

            var self = this;
            //  self.deleteConfirmed(id);
            swal.fire({
                title: 'Bu danışanı silmek istiyor musunuz?',
                text: '',
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Onay',
                cancelButtonText: 'İptal',
                reverseButtons: true,
                animation: false
            }).then((result) => {
                console.log(result);
                if (result.value) {
                    self.deleteConfirmed(id);
                }
            })
        },

        deleteConfirmed: function (id) {
            let self = this;
            console.log("deleteConfirme metodu", id);

            axios.post('/identity/companyuser/deleteconfirmed/' + id).then(function (response) {
                toastr.info("Danışan silindi!");
                let idx = self.tableConfig.data.findIndex(val => val.Id === id);
                self.tableConfig.data.splice(idx, 1);
            }).catch(function (error) {
                console.log(error);
            });
        }
    },
    created: function () {
        this.paginate(1, 20);
    },
    filters: {
        formatDate: function (value) {
            if (!value) return '';
            return moment(String(value)).format('DD/MM/YYYY');
        }
    }
})
