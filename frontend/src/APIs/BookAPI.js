import axios from 'axios';
import { toast } from 'react-toastify';

export default class BookAPI {
    constructor(accessToken) {
        if(accessToken === null){
            this.api = axios.create({
                headers: {
                    'Content-Type': 'application/json',
                },
            });
        }
        else {
            this.api = axios.create({
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                },
            });
        }
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

    // Book API Functions
    getBooks = async (params) => {
        try {
            const response = await this.api.get('/api/catalog/Book', { params });
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    getBook = async (id) => {
        try {
            const response = await this.api.get(`/api/catalog/Book/${id}`);
            return this.handleResponse(response);
        } catch (error) {
            toast.error(error.message);
        }
    }

    createBook = async (data) => {
        try {
            const response = await this.api.post('/api/catalog/Book', data);
            return this.handleResponse(response, 'Book created successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    updateBook = async (id, data) => {
        try {
            const response = await this.api.put(`/api/catalog/Book/${id}`, data);
            return this.handleResponse(response, 'Book updated successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    patchBook = async (id, data) => {
        try {
            const response = await this.api.patch(`/api/catalog/Book/${id}`, data);
            return this.handleResponse(response, 'Book patched successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }

    deleteBook = async (id) => {
        try {
            const response = await this.api.delete(`/api/catalog/Book/${id}`);
            return this.handleResponse(response, 'Book deleted successfully!');
        } catch (error) {
            toast.error(error.message);
        }
    }
}

