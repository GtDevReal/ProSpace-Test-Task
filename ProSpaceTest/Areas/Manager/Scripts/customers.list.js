const list = Vue.createApp({
    data() {
        return {
            isCreateCustomerModalOpen: false,
            isErrorModalOpen: false,
            isInfoModalOpen: false,
            isCreateUserModalOpen: false,
            isDeleteModalOpen: false,
            deleteMessage: '',
            infoMessage: '',
            errorMessage: '',
            customersList: [],
            token: '',
            customer: {
                id: '',
                name: '',
                address: '',
                discount: 0,
            },
            user: {
                id: '',
                customerId: '',
                email: '',
                password: '',
                phoneNumber: '',
            },
            validationCustomer: {
                name: '',
                address: '',
            },
            validationUser: {
                email: '',
                password: '',
            },
            currentPage: 0,
            itemsPerPage: 5,
            totalItems: 0,
        }
    },
    computed: {
        paginatedCustomers() {
            const start = (this.currentPage) * this.itemsPerPage;
            const end = start + this.itemsPerPage;
            return this.customersList.slice(start, end);
        },
        totalPages() {
            return Math.ceil(this.totalItems / this.itemsPerPage);
        }
    },
    methods: {
        async getCustomersList() {
            this.errorMessage = '';
            var url = "/distributors/l";

            try {
                var response = await axios.get(url);
                if (response.status != 200) {
                    this.infoMessage = response.data;
                    this.openModal(2);
                    return;
                }

                this.customersList = response.data;
                this.totalItems = this.customersList.length;

                this.currentPage = 0;
                this.$forceUpdate();
            }
            catch (e) {
                this.errorMessage = e.response.data;
                this.openModal(3);
            }
        },
        async createCustomer() {
            if (!this.validateCustomer()) {
                this.$forceUpdate();
                return;
            }

            if (this.customer.id == '') {
                var url = "/distributors/c";
                var token = $("input[name = '__RequestVerificationToken']").val();
                try {
                    const { id, ...customerCreate } = this.customer;
                    var response = await axios.post(url, customerCreate, {
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        }
                    })
                    this.closeModal(1);
                    if (response.status != 200) {
                        this.customer = { id: '', name: '', address: '', discount: 0, };
                        this.infoMessage = response.data;
                        this.openModal(2);
                        return;
                    }

                    this.customer = { id: '', name: '', address: '', discount: 0, };
                    this.infoMessage = 'Заказчик добавлен успешно!';
                    this.openModal(2);
                    this.currentPage = 0;
                    this.$forceUpdate();
                }
                catch (e) {
                    this.closeModal(1);
                    this.errorMessage = e.response.data;
                    this.openModal(3);
                    return;
                }
            }
            else {
                var url = "/distributors/e";
                var token = $("input[name = '__RequestVerificationToken']").val();
                try {
                    var response = await axios.post(url, this.customer, {
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        }
                    })
                    this.closeModal(1);
                    if (response.status != 200) {
                        this.customer = { id: '', name: '', address: '', discount: 0, };
                        this.infoMessage = response.data;
                        this.openModal(2);
                        return;
                    }

                    this.customer = { id: '', name: '', address: '', discount: 0, };
                    this.infoMessage = 'Данные обновлены успешно!';
                    this.openModal(2);
                    this.currentPage = 0;
                    this.$forceUpdate();
                }
                catch (e) {
                    this.closeModal(1);
                    this.errorMessage = e.response.data;
                    this.openModal(3);
                    return;
                }
            }
        },
        validateCustomer() {
            var result = true;
            this.validationCustomer = { name: '', address: '' };

            if (this.customer.name.length == 0) {
                this.validationCustomer.name = "Заполните поле 'Наименование'!";
                result = false;
            }
            if (this.customer.address.length == 0) {
                this.validationCustomer.address = "Заполните поле 'Адрес'!";
                result = false;
            }

            return result
        },
        async deleteCustomer(customerId) {
            var url = `/distributors/d/${customerId}`;
            try {
                var response = await axios.post(url);

                this.closeModal(5);

                if (response.status != 200) {
                    this.customer = { id: '', name: '', address: '', discount: 0, };
                    this.infoMessage = response.data;
                    this.openModal(2);
                    return;
                }

                this.user = { id: '', name: '', address: '', discount: 0, };
                this.infoMessage = 'Заказчик успешно удален!';
                this.openModal(2);
                this.currentPage = 0;
                this.$forceUpdate();
            }
            catch (e) {
                this.closeModal(5);
                this.errorMessage = e.response.data;
                this.openModal(3);
                return;
            }
        },
        async createUser() {
            if (!this.validateUser()) {
                this.$forceUpdate();
                return;
            }

            if (this.user.id == '') {
                var url = "/distributors/u/c";
                var token = $("input[name = '__RequestVerificationToken']").val();
                try {
                    const { id, ...userCreate } = this.user;
                    var response = await axios.post(url, userCreate, {
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        }
                    })
                    this.closeModal(4);
                    if (response.status != 200) {
                        this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                        this.infoMessage = response.data;
                        this.openModal(2);
                        return;
                    }

                    this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                    this.infoMessage = 'Пользователь добавлен успешно!';
                    this.openModal(2);
                    this.$forceUpdate();
                }
                catch (e) {
                    this.closeModal(4);
                    this.errorMessage = e.response.data;
                    this.openModal(3);
                    return;
                }
            }
            else {
                var url = "/distributors/u/e";
                var token = $("input[name = '__RequestVerificationToken']").val();
                try {
                    var response = await axios.post(url, this.user, {
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        }
                    })
                    this.closeModal(4);
                    if (response.status != 200) {
                        this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                        this.infoMessage = response.data;
                        this.openModal(2);
                        return;
                    }

                    this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                    this.infoMessage = 'Данные успешно отредактированы!';
                    this.openModal(2);
                    this.$forceUpdate();
                }
                catch (e) {
                    this.closeModal(4);
                    this.errorMessage = e.response.data;
                    this.openModal(3);
                    return;
                }
            }
        },
        validateUser() {
            var result = true;
            this.validationUser = { email: '', password: '' };

            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!this.user.email) {
                this.validationUser.email = "Email обязателен.";
                result = false;
            }
            else if (!emailPattern.test(this.user.email)) {
                this.validationUser.email = "Некорректный email.";
                result = false;
            }
            if (this.user.id == '') {
                if (!this.user.password) {
                    this.validationUser.password = "Пароль обязателен.";
                    result = false;
                }
                else if (this.user.password.length < 6) {
                    this.validationUser.password = "Пароль должен содержать минимум 6 символов.";
                    result = false;
                }
            }

            return result;
        },
        async deleteUser(userId) {
            var url = `/distributors/u/d/${userId}`;
            try {
                var response = await axios.post(url);

                this.closeModal(5);

                if (response.status != 200) {
                    this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                    this.infoMessage = response.data;
                    this.openModal(2);
                    return;
                }

                this.user = { id: '', customerId: '', email: '', password: '', phoneNumber: '' };
                this.infoMessage = 'Пользователь успешно удален!';
                this.openModal(2);
                this.$forceUpdate();
            }
            catch (e) {
                this.closeModal(5);
                this.errorMessage = e.response.data;
                this.openModal(3);
                return;
            }
        },
        openModal(selectedModal, user, customer) {
            switch (selectedModal) {
                case 1:
                    if (customer != '') {
                        this.customer = customer;
                    }

                    this.isCreateCustomerModalOpen = true;
                    break;
                case 2:
                    this.isInfoModalOpen = true;
                    break;
                case 3:
                    this.isErrorModalOpen = true;
                    break;
                case 4:
                    if (user != '') {
                        this.user = user;
                    }
                    else {
                        this.user.customerId = customer.id;
                    }

                    this.isCreateUserModalOpen = true;
                    break;
                case 5:
                    if (user != '') {
                        this.user.id = user.id;
                        this.deleteMessage = `Вы действительно хотите удалить пользователя ${user.email}?`;
                    }
                    else {
                        this.customer.id = customer.id;
                        this.deleteMessage = `Вы действительно хотите удалить заказчика ${customer.name}? После удаления, все привязанные пользователи тоже будут удалены!`;
                    }

                    this.isDeleteModalOpen = true;
                    break;
                default:
                    break;
            }
            this.$forceUpdate();
        },
        closeModal(selectedModal) {
            switch (selectedModal) {
                case 1:
                    this.isCreateCustomerModalOpen = false;
                    break;
                case 2:
                    this.isInfoModalOpen = false;
                    break;
                case 3:
                    this.isErrorModalOpen = false;
                    break;
                case 4:
                    this.isCreateUserModalOpen = false;
                    break;
                case 5:
                    this.isDeleteModalOpen = false;
                    break;
                default:
                    break;
            }
            this.getCustomersList();
        },
        nextPage() {
            this.currentPage++;
        },
        previousPage() {
            this.currentPage --;
        },

    },
    beforeMount() {
        this.getCustomersList()
    }
});
list.mount('#customers')