import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import { tableHandler } from '../sharedhandler/datatable-handler';
import '../../../../../node_modules/toastr/build/toastr.css';
import toastr from 'toastr';

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
            messages: []
        },
        tableConfig: {},
        urgencies: urgencies
    },
    methods: {

        paginate: function (pageNumber, pageLength) {
            var self = this;

            axios.get('/demand/getalloutgoingdemand', {
                params: {
                    pageNumber: pageNumber,
                    pageLength: pageLength,
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
        this.paginate(1, 10);
    },
    filters: {
    }
})
