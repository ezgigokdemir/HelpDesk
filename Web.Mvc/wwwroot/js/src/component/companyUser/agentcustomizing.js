import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import moment from 'moment';
import DatePicker from 'vue2-datepicker';
import { themeUtility } from "../../utilities/theme/theme-utility";
import _ from 'lodash';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';
import { tableHandler } from '../sharedhandler/datatable-handler';

toastr.options.progressBar = true;
toastr.options.positionClass = 'toast-bottom-right';

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
        'message': message,
        'multiselect': Multiselect
    },
    data: {
        config: {
            messages: [],
            showInfo: false,
            Province: {},
            District: {},
            search: '',
            selectionDisabled: false,
            selection: Number,
            searching: false,
            keyFilter: []
        },
        agents: agents,
        companies: companies,
        model: model
    },
    methods: {
        onSubmit: function () {
            this.$validator.validate().then(result => {
                if (!result) {
                    swal({
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
            
            if (self.config.keyFilter.length > 0) {
                self.model.ApplicationUsers = self.filteredData;
            } else {
                self.model.ApplicationUsers = null;
            }
            console.log("self.model.ApplicationUsers", self.model.ApplicationUsers);

            axios.post('/identity/companyuser/agentcustomizing', self.model).then(function (response) {

                self.config.messages.push({ type: 'success', content: 'Kayıt işlemi başarıyla tamamlandı!' });
                window.scrollTo({
                    top: 0,
                    left: 0,
                    behavior: "smooth"
                });
            }).catch(function (error) {
                if (error.response.status == 422) {
                    console.log("bilgi eksik");
                    self.config.messages.push({ type: 'warning', content: 'Lütfen form bilgilerini kontrol edin!' });
                } else {
                    console.log("kayıt başarısız");
                    self.config.messages.push({ type: 'warning', content: 'Kayıt işlemi gerçekleştirilemedi!' });
                }
                window.scrollTo({
                    top: 0,
                    left: 0,
                    behavior: "smooth"
                });
                console.log(error);
            });
        },

        getUserForCompany: function (companyId) {
            let self = this;
            axios.get('/identity/companyuser/getuserforcompany?companyId=' + companyId)
                .then(function (response) {
                    console.log("response", response);
                    self.agents = response.data;
                })
                .catch(function (error) {
                    console.log(error);
                })
                .then(function () {

                });
        }
    },

    created: function () {

    },

    computed: {

        filteredData: function () {
            let data = this.agents;
            console.log("baseData: ", data);
            console.log("this.config.keyFilter", this.config.keyFilter);
            if (this.config.keyFilter.length > 0) {
                data = data.filter(val => this.config.keyFilter.indexOf(val.FirstName.concat(' ' + val.LastName)) > -1);
            }
            console.log("filtereddata 2", data);
            return data;
        },

        agentList: function () {
            return this.agents.map(val => val.FirstName.concat(' ' + val.LastName));
        }
    },

    filters: {
    }
});
