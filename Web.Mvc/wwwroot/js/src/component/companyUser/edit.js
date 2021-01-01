import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import _ from 'lodash';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';

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
        'message': message
    },
    data: {
        config: {
            messages: [],
            keyFilter: []
        },
        companyUserViewModel: companyUserViewModel,
        //companies: companies,
        //currentRoles: currentRoles,
        //  companySelect: {},
        //userAddedCount: userAddedCount,
        //userLimit: userLimit,
        selections: [{ Name: 'Firma Admin', IsChecked: false }, { Name: 'Firma Kullanıcısı', IsChecked: false }]
    },
    methods: {
        onSubmit: function () {
            console.log("onsubmit çalıştı");
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
            console.log("submit çalıştı");
            console.log("self.companyUserViewModel", self.companyUserViewModel);
            //self.companyUserViewModel.UserDto.CompanyId = self.companySelect.Id;
            //self.companyUserViewModel.CompanyId = self.companySelect.Id;
            //self.companyUserViewModel.Company = self.filteredData;
            // console.log(self.companyUserViewModel);
            // self.companyUserViewModel.UserDto.IsCompanyUserLimitExceeded = true;

            axios.post('/identity/companyuser/edit', self.companyUserViewModel)
                .then(function (response) {

                    console.log("kayıt başarılı");
                    //self.companyUserViewModel.UserDto = {};
                    //self.companyUserViewModel.IdentityUserId = '';
                    //self.companyUserViewModel.Password = '';


                    self.config.messages.push({ type: 'success', content: 'Kayıt işlemi başarıyla tamamlandı! Listeye dönmek için <a href="/identity/companyuser/index" class="btn btn-info" target="_self">tıklayınız</a>' });
                    window.scrollTo({
                        top: 0,
                        left: 0,
                        behavior: "smooth"
                    });
                }).catch(function (error) {

                    self.config.messages.push({ type: 'warning', content: 'Kayıt işlemi gerçekleştirilemedi, aynı eposta adresi ile daha önce hesap açılmadığından emin olun.' });
                    window.scrollTo({
                        top: 0,
                        left: 0,
                        behavior: "smooth"
                    });
                    console.log(error);


                });

        }
    },

    created: function () {
    },

    computed: {
    },

    mounted: function () {
        // this.$nextTick(this.companyCapacityAlert);
    },
    filters: {
    }
});
