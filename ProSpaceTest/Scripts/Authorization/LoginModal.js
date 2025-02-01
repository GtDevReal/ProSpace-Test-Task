const loginApp = Vue.createApp({
	data() {
		return {
			isModalOpen: true,
			antiForgeryToken: "",
			loginForm: {
				email: '',
				password: '',
				rememberMe: false
			},
			errors: {
				email: '',
				password: '',
				response: ''
			},
		};
	},
	methods: {
		submitForm() {
			this.errors = { email: '', password: '' };

			const form = document.getElementById('login-form');
			if (!form.checkValidity()) {
				this.showValidationErrors(form);
				this.$forceUpdate();
				return;
			}

			this.login();
		},
		showValidationErrors(form) {
			Array.from(form.elements).forEach((element) => {
				if (element.validity && !element.validity.valid) {
					this.errors[element.id] = element.validationMessage;
				}
			});
		},
		async login() {
			try {
				var url = "/Login"
				var token = $("input[name = '__RequestVerificationToken']").val()
				var response = await axios.post(url, this.loginForm, {
					headers: {
						'Content-Type': 'application/json',
						'RequestVerificationToken': token
					}
				}).then(response => {
					window.location.href = response.data;
				})
			}
			catch (error) {
				this.errors.response = error.response?.data || "Ошибка запроса на сервер!";
			}
		}
	},
});

loginApp.mount("#login-modal");
