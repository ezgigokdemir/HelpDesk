import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import DatePicker from 'vue2-datepicker';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import _ from 'lodash';

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
            keyFilter: []
        },
        userViewModel: userViewModel,
        companies: companies,
        selections: [{ Name: 'Admin', IsChecked: false }, { Name: 'Temsilci', IsChecked: false }],
        companyRecordSelection: 0
    },
    methods: {
        onSubmit: function () {
            var self = this;
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
            if (self.filteredData !== undefined) {
                self.userViewModel.Companies = self.filteredData;
            }
            self.userViewModel.RecordSelection = self.companyRecordSelection;

            axios.post('/identity/user/edit', self.userViewModel).then(function (response) {
                console.log("self.userViewModel.UserRoles", self.userViewModel.UserRoles);
                self.userViewModel.UserDto = {};

                self.config.messages.push({ type: 'success', content: 'Kayıt işlemi başarıyla tamamlandı! Listeye dönmek için <a href="/identity/user/index" class="btn btn-info" target="_self">tıklayınız</a>' });
                window.scrollTo({
                    top: 0,
                    left: 0,
                    behavior: "smooth"
                });
                self.config.showForm = false;
            }).catch(function (error) {
                console.log(error);
                toastr.error("Bir hata oluştu!");
            }).then(function () {
                
            });
        },

        showCompanySelection: function (roleDisplayName, selection) {
            let self = this;
            if (roleDisplayName === 'Admin') {
                self.selections[0].IsChecked = selection
            }
            if (roleDisplayName === 'Temsilci') {
                self.selections[1].IsChecked = selection
            }

            if (((self.selections[0].IsChecked === true) && (self.selections[1].IsChecked === false)) ||
                (self.selections[0].IsChecked === false) && (self.selections[1].IsChecked === false)) {
                this.config.keyFilter = [];
                document.getElementById("companySelection").style.display = "none";
            }
            else {
                document.getElementById("companySelection").style.display = "block";
            }
        },

        companySelectionControl: function () {
            console.log("roles", self.userViewModel.UserRoles);

            var condition = (self.userViewModel.UserRoles[1].RoleName === 'Agent' && self.userViewModel.UserRoles[1].Selected === true);
            console.log("koşul", condition);
            if (condition) {
                document.getElementById("companySelection").style.display = "block";
            }
        }
    },

    computed: {
        filteredData: function () {
            let data = Object.entries(this.companies);
            if (this.config.keyFilter.length > 0) {
                console.log("filter 1", (data[1])[1]);
                var newData = (data[1])[1];
                newData = newData.filter(val => this.config.keyFilter.indexOf(val.CompanyName) > -1);
            }
            console.log("filtereddata", newData);
            return newData;
        },

        companyList: function () {
            console.log("companyRecordSelection", this.companyRecordSelection);
            return this.companies.data.map(val => val.CompanyName);
        }
    },

    mounted: function () {
        this.$nextTick(this.companySelectionControl);
    },

    filters: {
    }
});
