import React, {useEffect, useState} from 'react';
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from '@mui/material';
import CategoryAPI from "../APIs/CategoryAPI";
import useAPI from "../CustomHooks/useAPI";

const CategoryModal = ({ open, setOpen, category = {} }) => {
    const [name, setName] = useState(category.name || '');
    const categoryAPI = useAPI(CategoryAPI);
    const handleSave = async () => {
        if (category.name) {
            // Edit existing category
            const editCategoryDTO = { Name: name };
            await categoryAPI.updateCategory(category.id, editCategoryDTO);
        } else {
            // Create new category
            const createCategoryDTO = { Name: name };
            await categoryAPI.createCategory(createCategoryDTO);
        }
        setOpen(false);  // Close the modal after saving
    };

    useEffect(() => {
        setName(category.name || '');
    }, [category])

    return (
        <Dialog open={open} onClose={() => setOpen(false)}>
            <DialogTitle>{category.name ? 'Edit Category' : 'Create Category'}</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Category Name"
                    type="text"
                    fullWidth
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={() => setOpen(false)} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleSave} color="primary">
                    Save
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default CategoryModal;
