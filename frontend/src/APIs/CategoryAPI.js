import axios from 'axios';
import { toast } from 'react-toastify';

export default class CategoryAPI {
    constructor(accessToken) {
        this.api = axios.create({
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${accessToken}`,
            },
        });
    }

    async handleResponse(response, successMessage) {
        if (response.status >= 200 && response.status <= 299) {
            toast.success(successMessage);
            return response.data;
        } else {
            toast.error(`Error: ${response.status}`);
            throw new Error(`Error: ${response.status}`);
        }
    }

    // Category API Functions
    getCategories = async (params) => {
        try {
            const response = await this.api.get('/api/catalog/Category', { params });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    getCategory = async (id) => {
        try {
            const response = await this.api.get(`/api/catalog/Category/${id}`);
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    createCategory = async (data) => {
        try {
            const response = await this.api.post('/api/catalog/Category', data);
            return this.handleResponse(response, 'Category created successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    updateCategory = async (id, data) => {
        try {
            const response = await this.api.put(`/api/catalog/Category/${id}`, data);
            return this.handleResponse(response, 'Category updated successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    patchCategory = async (id, data) => {
        try {
            const response = await this.api.patch(`/api/catalog/Category/${id}`, data);
            return this.handleResponse(response, 'Category patched successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    deleteCategory = async (id) => {
        try {
            const response = await this.api.delete(`/api/catalog/Category/${id}`);
            return this.handleResponse(response, 'Category deleted successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }
}

