const list = Vue.createApp({
	data() {
		return {
			isErrorModalOpen: false,
			isCreateModalOpen: false,
			isDeleteModalOpen: false,
			deleteGuid: '',
			products: [],
			categories: [],
			errorMessage: '',
			pageNumber: 0,
			size: 9,
			productModel: {
				id: '',
				code: '',
				name: '',
				price: 0,
				category: '',
			},
			errorsProduct: {
				code: '',
				name: '',
				price: '',
				category: '',
			},
			productFilters: {
				category: '',
				minPrice: null,
				maxPrice: null,
				sortBy: 'name',
				sortDirection: 'asc',
			},
		}
	},
	computed: {
		findMinPrice() {
			if (this.filteredProducts.length === 0) return 0;
			return Math.min(...this.filteredProducts.map(product => product.price));
		},
		findMaxPrice() {
			if (this.filteredProducts.length === 0) return 0;
			return Math.max(...this.filteredProducts.map(product => product.price));
		},
		filteredProducts() {
			let filtered = this.products;

			// Фильтрация по категории
			if (this.productFilters.category) {
				filtered = filtered.filter(product => product.category.toLowerCase() === this.productFilters.category.toLowerCase());
			}

			if (this.productFilters.minPrice !== null) {
				filtered = filtered.filter(product => product.price >= this.productFilters.minPrice
				);
			}
			if (this.productFilters.maxPrice !== null) {
				filtered = filtered.filter(product => product.price <= this.productFilters.maxPrice
				);
			}

			// Сортировка
			if (this.productFilters.sortBy) {
				filtered.sort((a, b) => {
					const valueA = a[this.productFilters.sortBy];
					const valueB = b[this.productFilters.sortBy];

					if (valueA < valueB) {
						return this.productFilters.sortDirection === 'asc' ? -1 : 1;
					}
					if (valueA > valueB) {
						return this.productFilters.sortDirection === 'asc' ? 1 : -1;
					}
					return 0;
				});
			}

			return filtered;
		},
		paginatedData() {
			const start = this.pageNumber * this.size;
			const end = start + this.size;
			return this.filteredProducts.slice(start, end);
		},
		pageCount() {
			return Math.ceil(this.filteredProducts.length / this.size);
		}
	},
	methods: {
		async getProductList() {
			var url = '/dash/p';
			try {
				var response = await axios.get(url);

				if (response.status != 200) {
					this.products = [];
					this.showModalError(response);
				}

				this.errorMessage = '';
				this.products = response.data;
				this.categories = [...new Set(this.products.map(product => product.category.toLowerCase()))];
			}
			catch (e) {
				this.products = [];
				this.errorMessage = e;
				this.$forceUpdate();
			}
		},
		nextPage() {
			if (this.pageNumber < this.pageCount - 1) {
				this.pageNumber++;
			}
		},
		previousPage() {
			if (this.pageNumber > 0) {
				this.pageNumber--;
			}
		},
		openCreateProductModal() {
			this.isCreateModalOpen = true;
		},
		closeCreateProductModal() {
			this.isCreateModalOpen = false;
		},
		async createProduct() {
			var result = this.validateProduct();
			if (!result) {
				this.$forceUpdate();
				return;
			}

			if (this.productModel.id != '' && this.productModel.id != null) {
				try {
					var url = "/product/e"
					var token = $("input[name = '__RequestVerificationToken']").val()
					var response = await axios.post(url, this.productModel, {
						headers: {
							'Content-Type': 'application/json',
							'RequestVerificationToken': token
						}
					})
					if (response.status == 200) {
						this.productModel = { id: '', code: '', name: '', price: 0, category: '' };
						this.closeCreateProductModal();
						this.$forceUpdate();
						this.getProductList();
					}
					else {
						this.productModel = { id: '', code: '', name: '', price: 0, category: '' };
						this.showModalError(response)
					}
				}
				catch (e) {
					this.showModalError(e);
				}
			}
			else {
				try {
					const { id, ...productCreate } = this.productModel;

					var url = "/product/a"
					var token = $("input[name = '__RequestVerificationToken']").val()
					var response = await axios.post(url, productCreate, {
						headers: {
							'Content-Type': 'application/json',
							'RequestVerificationToken': token
						}
					})
					if (response.status == 200) {
						this.productModel = { code: '', name: '', price: 0, category: '' };
						this.closeCreateProductModal();
						this.$forceUpdate();
						this.getProductList();
					}
					else {
						this.showModalError(response);
					}
				}
				catch (e) {
					this.showModalError(e);
				}
			}
		},
		validateProduct() {
			var result = true;
			this.errorsProduct = { code: '', name: '', price: '', category: '' };

			if (this.productModel.name.length == 0) {
				this.errorsProduct.name = "Заполните поле 'Наименование'!";
				result = false;
			}
			if (this.productModel.code.length == 0) {
				this.errorsProduct.code = "Сгенерируйте код продукта!";
				result = false;
			}
			if (this.productModel.category.length == 0) {
				this.errorsProduct.category = "Заполните поле 'Категория'!";
				result = false;
			}
			if (this.productModel.price == 0) {
				this.errorsProduct.price = "Стоимость не может быть равна 0!";
				result = false;
			}

			return result
		},
		generateCode() {
			const randomNumber = () => Math.floor(Math.random() * 10);
			const randomLetter = () => String.fromCharCode(65 + Math.floor(Math.random() * 26));

			const part1 = `${randomNumber()}${randomNumber()}`;
			const part2 = `${randomNumber()}${randomNumber()}${randomNumber()}${randomNumber()}`;
			const part3 = `${randomLetter()}${randomLetter()}`;
			const part4 = `${randomNumber()}${randomNumber()}`;

			this.productModel.code = `${part1}-${part2}-${part3}${part4}`;
			this.$forceUpdate();
		},
		resetProductFilters() {
			this.productFilters = {
				category: '',
				minPrice: null,
				maxPrice: null,
				sortBy: 'name',
				sortDirection: 'asc',
			};
			this.getProductList();
		},
		setSort(sortBy, sortDirection) {
			this.productFilters.sortBy = sortBy;
			this.productFilters.sortDirection = sortDirection;
		},
		async getProductInfo(id) {
			try {
				this.productModel = { id: '', code: '', name: '', price: 0, category: '' };
				var url = `/product/i/${id}`
				var response = await axios.get(url);
				if (response.status != 200) {
					this.showModalError(response)
				}

				this.productModel = response.data;
				this.isCreateModalOpen = true;
			}
			catch (e) {
				this.showModalError(e);
			}
		},
		showModalError(message) {
			this.isErrorModalOpen = true;
			this.errorMessage = message;
			this.$forceUpdate();
			return;
		},
		closeErrorModal() {
			this.isErrorModalOpen = false;
		},
		deleteConfirm(id) {
			this.isDeleteModalOpen = true;
			this.deleteGuid = id;
		},
		closeDeleteModal() {
			this.isDeleteModalOpen = false;
		},
		async deleteProduct(id) {
			try {
				this.closeDeleteModal();
				var url = `/product/d/${id}`;
				var response = await axios.delete(url);
				if (response.status == 200) {
					this.deleteGuid = '';
					this.$forceUpdate();
					this.getProductList();
				}
				else {
					this.showModalError(response);
				}
			}
			catch (e) {
				this.showModalError(e);
			}
		}
	},
	beforeMount() {
		this.getProductList();
	}
})
list.mount('#dash');