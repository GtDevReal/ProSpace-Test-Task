const logout = Vue.createApp({
	data() {
        return {
            isModalOpen: false
		};
	},
    methods: {
        openModal() {
            this.isModalOpen = true;
        },
        closeModal() {
            this.isModalOpen = false;
        },
        logout() {
            axios.post("/Home/Logout")
                .then(response => {
                    window.location.href = response.data;
                })
            this.closeModal();
        }
    }
});

logout.mount('#app');

