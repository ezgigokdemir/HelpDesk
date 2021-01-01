import Vue from 'vue';
import swal from 'sweetalert2';
import axios from 'axios';
import VeeValidate, { Validator } from 'vee-validate';
import tr from 'vee-validate/dist/locale/tr';
import VueTheMask from 'vue-the-mask';
import message from '../shared/message.vue';
import toastr from 'toastr';
import '../../../../../node_modules/toastr/build/toastr.css';
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
        'message': message
    },
    data: {
        config: {
            messages: [],
            showInfo: false,
            showForm: true
        },
        demandDto: demandDto
    },
    methods: {
        onSubmit: function () {
            var self = this;
            self.submit();
        },

        submit: function () {
            let self = this;

            if (self.demandDto.IsAccepted === true) {
                self.config.messages.push({ type: 'warning', content: 'Bu talebin bir kullanıcıya ataması yapılmıştır, kaydı silemezsiniz!' });
            } else {
                axios.post('/demand/deleteconfirmedasync?id=' + self.demandDto.Id).then(function (response) {

                    self.demandDto = {};

                    self.config.messages.push({ type: 'success', content: 'Talep silme işlemi başarıyla tamamlandı!' });
                    window.scrollTo({
                        top: 0,
                        left: 0,
                        behavior: "smooth"
                    });
                    self.config.showForm = false;
                }).catch(function (error) {
                    console.log(error);
                    toastr.error("Kayıt silme işlemi sırasında hata oluştu!");
                }).then(function () {

                });
            }
        }
    },

    computed: {

    },

    mounted: function () {

    },

    filters: {
    }
});
