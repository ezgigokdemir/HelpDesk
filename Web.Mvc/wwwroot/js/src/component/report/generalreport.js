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
            datepickerLang: {
                days: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'],
                months: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara'],
                pickers: ['sonraki 7 gün', 'sonraki 30 gün', 'önceki 7 gün', 'önceki 30 gün'],
                placeholder: {
                    date: 'Tarih Seçiniz',
                    dateRange: 'Tarih Aralığı Seçiniz'
                }
            },
            keyFilter: [],
            taskStatusfilterKey: 'notAssigned'
        },
        tableConfig: {},
        reportTypes: [],
        reportData: [],
        users: users.data,


        excelTemp: {
            'Id':'Key',
            'Başlık': 'Name',
            'Toplam': 'Count',
        },
        excelData:[],
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

        refreshReport: function () {
            this.$refs.calendar.$emit('refetch-events');
        },

        submit: function () {
            let self = this;
            this.reportData = [];
            console.log("start , end", moment(self.config.startDate).format('YYYY-MM-DD'), moment(self.config.endDate).format('YYYY-MM-DD'));
            axios.get('/report/getuserreportdata', {
                params: {
                    start: moment(self.config.startDate).format('YYYY-MM-DD'),
                    end: moment(self.config.endDate).format('YYYY-MM-DD'),
                    reportFilterType: self.config.reportFilterType,
                    filterParameter: self.config.userFilter,
                    taskStatusfilterKey: self.config.taskStatusfilterKey,
                    //filteredData=this.filteredData,
                }
            }).then(function (response) {
                console.log(response);
                
                self.reportData = response.data;
                self.excelData = response.data;
                self.excelData.Key = response.data.Key;
                self.excelData.Name = response.data.Name;
                self.excelData.Count = response.data.Count;
                console.log("response self.reportData", self.reportData);
                console.log("response  self.json_data ", self.excelData);
                console.log("response  self.json_data.name ", self.excelData.Name);
            }).catch(function (error) {
                console.log(error);
            }).then(function () {
            });
            // post
        },



    },
    watch: {
     
    },

    created: function () {

    },

    computed: {
        filteredData: function () {
            let data = this.reportData;

            //console.log(" data", data);
            //console.log("this.config.keyFilter", this.config.keyFilter);

            if (this.config.keyFilter.length > 0) {
                data = data.filter(val => this.config.keyFilter.indexOf(val.Name) > -1);
                //  data = data.filter(val => this.config.keyFilter.indexOf(val.FirstName.concat(' ' + val.LastName)) > -1);
                console.log("filteredData  if ici", data);
            }

            return data;
        },

        total: function () {
            let data = Object.entries(this.reportData);
            if (this.config.keyFilter.length > 0) {
                data = data.filter(val => this.config.keyFilter.indexOf(val.FirstName) > -1);
                return data.map(val => val[1]).reduce((accumulator, currentValue) => accumulator + currentValue);
            }
            else {
                //console.log(data.map(val => val[1]));
                return data.map(val => val[1]).reduce((accumulator, currentValue) => accumulator + currentValue);
            }
        },

        userList: function () {
            console.log("users.data js", users.data);
            return this.users.map(val => val.FirstName.concat(' ' + val.LastName));
        }

    },

    filters: {
    },
    mounted: function () {
        
    }
});

