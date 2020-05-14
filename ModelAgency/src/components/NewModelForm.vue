<template>
  <div class="cc col-12">
    <div class="row">
      <div class="col-4">
        <h1>New Model</h1>
        <form @submit.prevent="checkForm">
          <div class="row">
            <div class="col-5 form-group">
              <label for="firstName">First name</label>
              <input class="form-control" v-model="form.firstName" name="firstName" />
            </div>
            <div class="col-5 form-group">
              <label for="lastName">Last name</label>
              <input class="form-control" v-model="form.lastName" name="lastName" />
            </div>
          </div>
          <div class="row">
            <div class="col-10 form-group">
              <label for="email">Email</label>
              <input class="form-control" v-model="form.email" name="email" />
            </div>
          </div>
          <div class="row">
            <div class="col-10 form-group">
              <label for="password">Password</label>
              <input class="form-control" v-model="form.password" name="password" type="password" />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <label for="addressLine1">Address line 1</label>
              <input class="form-control" v-model="form.addressLine1" name="addressLine1" />
            </div>
            <div class="col-5 form-group">
              <label for="addressLine2">Address line 2</label>
              <input class="form-control" v-model="form.addressLine2" name="addressLine2" />
            </div>
          </div>
          <div class="row">
            <div class="col-3 form-group">
              <label for="zip">Zip code</label>
              <input class="form-control" v-model="form.zip" name="zip" />
            </div>
            <div class="col-7 form-group">
              <label for="city">City</label>
              <input class="form-control" v-model="form.city" name="city" />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <label for="country">Country</label>
              <input class="form-control" v-model="form.country" name="country" />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <label for="birthday">Birthday</label>
              <input class="form-control" v-model="form.birthday" name="birthday" />
            </div>
            <div class="col-5 form-group">
              <label for="nationality">Nationality</label>
              <input class="form-control" v-model="form.nationality" name="nationality" />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <label for="height">Height</label>
              <input class="form-control" v-model.number="form.height" name="height" type="number" />
            </div>
            <div class="col-5 form-group">
              <label for="shoeSize">Shoe size</label>
              <input
                class="form-control"
                v-model.number="form.shoeSize"
                name="shoeSize"
                type="number"
              />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <label for="haircolor">Hair color</label>
              <input class="form-control" v-model="form.hairColor" name="hairColor" />
            </div>
            <div class="col-5 form-group">
              <label for="eyeColor">Eye Color</label>
              <input class="form-control" v-model="form.eyeColor" name="eyeColor" />
            </div>
          </div>
          <div class="row">
            <div class="col-10 form-group">
              <label for="comments">Comments</label>
              <input class="form-control" v-model="form.comments" name="comments" />
            </div>
          </div>
          <div class="row">
            <div class="col-5 form-group">
              <input type="submit" class="btn btn-success" value="Create new model" />
            </div>
          </div>
        </form>
      </div>
      <div class="col-4">
        <span v-show="errors.length">
          <b>Required:</b>
          <ul>
            <li v-for="error in errors" :key="error.message">{{error.message}}</li>
          </ul>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      errors: [],
      form: {
        firstName: "",
        lastName: "",
        email: "",
        addressLine1: "",
        addressLine2: "",
        zip: "",
        city: "",
        country: "",
        birthday: "",
        nationality: "",
        height: 0,
        shoeSize: 0,
        hairColor: "",
        eyeColor: "",
        comments: "",
        password: ""
      }
    };
  },
  methods: {
    checkForm(e) {
      if (
        this.form.firstName &&
        this.form.lastName &&
        this.form.email &&
        this.form.password
      ) {
        let url = "https://localhost:44368/api/models";
        fetch(url, {
          method: "POST",
          credentials: "include",
          body: JSON.stringify(this.form),
          headers: {
            Authorization: "Bearer " + localStorage.getItem("token"),
            "Content-Type": "application/json"
          }
        })
          // eslint-disable-next-line no-console
          .catch(error => () => console.log(error));
      }
      this.errors = [];

      if (!this.form.firstName) {
        this.errors.push({ message: "First name" });
      }
      if (!this.form.lastName) {
        this.errors.push({ message: "Last name" });
      }
      if (!this.form.email) {
        this.errors.push({ message: "Email" });
      }
      if (!this.form.password) {
        this.errors.push({ message: "Password" });
      }
      e.PreventDefault();
    }
  }
};
</script>

<style scoped>
.cc {
  margin: 20px;
}
</style>