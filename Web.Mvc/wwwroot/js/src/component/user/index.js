import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import { tableHandler } from '../sharedhandler/datatable-handler';
import moment from 'moment';
import '../../../../../node_modules/toastr/build/toastr.css';
import toastr from 'toastr';


toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

Vue.use(VueTheMask);
Vue.use(VeeValidate);

Vue.use(VueTheMask);
Vue.use(VeeValidate);

Validator.localize('tr', tr);

VeeValidate.Validator.extend('verify_password', {
    getMessage: field => `Şifre: 1 büyük harf, 1 küçük harf, 1 rakam ve bir özel karakter (Örn. , . _ & ? vb.)`,
    validate: value => {
        var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?:{}|<>\*])(?=.{8,})");
        return strongRegex.test(value);
    }
});

var app = new Vue({
    el: '#app',
    components: {
        'message': message
    },
    data: {
        config: {
            messages: [],
        },
        users: users,
        tableConfig: {},
        searchType: 0,
        searchString: ''
    },
    methods: {

        paginate: function (pageNumber, pageLength, searchType, searchString) {
            var self = this;
            axios.get('/identity/user/getall', {
                params: {
                    pageNumber: pageNumber,
                    pageLength: pageLength,
                    searchType: self.searchType,
                    searchString: self.searchString
                }
            })
                .then(function (response) {
                    console.log(response);
                    tableHandler.handleResponse(self, response, pageNumber, pageLength);
                })
                .catch(function (error) {
                    console.log(error);
                })
                .then(function () {

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
