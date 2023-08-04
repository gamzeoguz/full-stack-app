<template>
    <div>
<table class="table table-hover table-striped">
    <thead>
      <tr>
        <th scope="col">ID</th>
        <th scope="col">Name</th>
        <th scope="col">Email</th>
        <th scope="col">Update</th>
        <th scope="col">Delete</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="user in items" :key="user.id">
        <th scope="row">{{ user.id }}</th>
        <td>{{ user.name }}</td>
        <td>{{ user.email }}</td>
        <td><a @click="onClickOpen(user.id)" class="text-warning" data-toggle="modal" href="#myModal">Update</a></td>
        <td><a @click="deleteUser(user.id)" class="text-danger">Delete</a></td>
      </tr>
    </tbody>
  </table>

<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h4 class="modal-title" id="myModalLabel">Update user</h4>
      </div>
      <div class="modal-body">
        <input type="text" v-model="user.name" v-validate="'required'" name="name" class="form-control" :class="{ 'is-invalid': submitted && errors.has('name') }" />
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal" v-on:click="onClickClose()">Close</button>
        <button type="button" class="btn btn-primary" v-on:click="updateUser(user.id)">Save changes</button>
      </div>
    </div>
  </div>
</div>
</div>
</template>

<script>
import axios from "axios";

export default {
    created() {
        this.getUsers();
    },
    data() {
        return {
            items: [],
            user: {
                id: '',
                name: ''
            },
        }
    },
    methods: {
        getUsers() {
            axios
                .get("https://localhost:7282/TestUser/Get")
                .then((response) => {
                    console.log(response.data);
                    this.items = response.data;
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        deleteUser(id) {
            axios
                .delete("https://localhost:7282/TestUser/Remove/"+ id)
                .then((response) => {
                    console.log(response.data);
                    this.items = this.getUsers();
                })
                .catch((error) => {
                    console.log(error);
                });
        },

        updateUser(id) {
            axios
                .put("https://localhost:7282/TestUser/Update", {
                    name: this.user.name,
                    id: this.user.id
                })
                .then((response) => {
                    this.items = this.getUsers();
                    this.onClickClose();
                })
                .catch((error) => {
                    console.log(error);
                });
            },

        onClickOpen(id) {
            $('#myModal').modal('show');
            axios
                .get("https://localhost:7282/TestUser/GetUser/" + id)
                .then((response) => {
                    console.log(response.data);
                    this.user.name = response.data.name;
                    this.user.id = response.data.id;
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        onClickClose() {
            $('#myModal').modal('hide');
        },

    }
};
</script>
