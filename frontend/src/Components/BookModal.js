import React, { useEffect, useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    TextField,
    DialogActions,
    Button,
    Select,
    InputLabel,
    MenuItem,
    FormControl,
} from '@mui/material';
import BookAPI from "../APIs/BookAPI";
import CategoryAPI from "../APIs/CategoryAPI";
import useAPI from "../CustomHooks/useAPI";

const BookModal = ({ open, setOpen, book = {} }) => {
    const [isbn, setIsbn] = useState(book.isbn || '');
    const [title, setTitle] = useState(book.title || '');
    const [description, setDescription] = useState(book.description || '');
    const [stock, setStock] = useState(book.stock || 0);
    const [categoryId, setCategoryId] = useState(book?.category?.categoryId || '');
    const [categories, setCategories] = useState([]);

    const bookAPI = useAPI(BookAPI);
    const categoryAPI = useAPI(CategoryAPI);

    useEffect(() => {
        setIsbn(book.isbn || '');
        setTitle(book.title || '');
        setDescription(book.description || '');
        setStock(book.stock || 0);
        setCategoryId(book?.category?.categoryId || '');
    }, [book]);

    useEffect(() => {
        const fetchCategories = async () => {
            const categoriesData = await categoryAPI?.getCategories();
            setCategories(categoriesData);
        };
        fetchCategories();
    }, [categoryAPI]);

    const handleSave = async () => {
        const bookDTO = {
            isbn: isbn,
            Title: title,
            Description: description,
            Stock: parseInt(stock,10),
            CategoryId: categoryId || null,
        };
        if (book.title) {
            // Edit existing book
            await bookAPI.updateBook(book.isbn, bookDTO);
        } else {
            // Create new book
            await bookAPI.createBook(bookDTO);
        }
        setOpen(false);  // Close the modal after saving
    };
    return (
        <Dialog open={open} onClose={() => setOpen(false)}>
            <DialogTitle>{book.title ? 'Edit Book' : 'Create Book'}</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="ISBN"
                    type="text"
                    fullWidth
                    value={isbn}
                    onChange={(e) => setIsbn(e.target.value)}
                />
                <TextField
                    autoFocus
                    margin="dense"
                    label="Title"
                    type="text"
                    fullWidth
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                />
                <TextField
                    margin="dense"
                    label="Description"
                    type="text"
                    fullWidth
                    multiline
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />
                <TextField
                    margin="dense"
                    label="Stock"
                    type="number"
                    fullWidth
                    value={stock}
                    onChange={(e) => setStock(e.target.value)}
                />
                <FormControl fullWidth margin="dense">
                    <InputLabel>Category</InputLabel>
                    <Select
                        value={categoryId}
                        onChange={(e) => setCategoryId(e.target.value)}
                    >
                        {categories?.map((category, index) => (
                            <MenuItem key={index} value={category.categoryId}>
                                {category.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={() => setOpen(false)} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleSave} color="primary">
                    {book.title ? 'UPDATE' : 'SAVE'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default BookModal;
