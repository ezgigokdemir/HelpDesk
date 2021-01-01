import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import moment from 'moment';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';
import { themeUtility } from "../../utilities/theme/theme-utility";
import { tableHandler } from '../sharedhandler/datatable-handler';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import excel from 'vue-excel-export';

toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

Vue.use(VueTheMask);
Vue.use(VeeValidate);
Vue.use(excel);
Validator.localize('tr', tr);

var app = new Vue({
    el: '#app',
    components: {
        'message': message,
        'date-picker': DatePicker,
        'multiselect': Multiselect
    },
    data: {
        config: {
            messages: [],
            showInfo: false,
            taskStatusfilterKey: 'notAssigned'
        },
        tableConfig: {},
        reportTypes: [],
        reportData: [],

        excelTemp: {
            'Id': 'Key',
            'Başlık': 'Name',
            'Toplam': 'Count',
        },
        excelData: [],
        excelmeta: [
            [
                {
                    'key': 'charset',
                    'value': 'utf-8'
                }
            ]
        ],
    },
    methods: {
        // validate and submit
        onSubmit: function () {
            var self = this;
            this.$validator.validate().then(result => {
                if (!result) {
                    swal.fire({
                        type: '',
                        title: '',
                        text: 'Lütfen form bilgilerini kontrol edin!',
                        animation: false
                    });
                } else {
                    this.submit();
                }
            });
        },

        submit: function () {
            let self = this;
            this.reportData = [];

            axios.get('/report/getmonthlyreportdata', {
                params: {
                    year: self.config.year,
                    month: self.config.month,
                    taskStatusfilterKey: self.config.taskStatusfilterKey
                }
            }).then(function (response) {
                console.log(response);
                self.reportData = response.data;
                self.excelData = response.data;
                self.excelData.Key = response.data.Key;
                self.excelData.Name = response.data.Name;
                self.excelData.Count = response.data.Count;
                console.log("self.reportData", self.reportData);
                console.log("self.reportData", response);
            }).catch(function (error) {
                console.log(error);
            }).then(function () {

            });
            // post
        },

    },

    created: function () {
    },

    computed: {
    },

    filters: {
    }
});

