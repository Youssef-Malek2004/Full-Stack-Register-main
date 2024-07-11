import React, { useState } from "react";
import backgroundImage from "../Components/background2.png";

function CreatingForm() {
  const [formData, setFormData] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    birthDate: null,
    mobileNumber: "",
    email: "",
    addressList: [
      {
        governate: "",
        city: "",
        street: "",
        buildingNumber: "",
        flatNumber: "",
      },
    ],
  });

  const [confirmationPopup, setConfirmationPopup] = useState(false);

  const handleSubmit = (event) => {
    event.preventDefault();
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  return (
    <>
      <div
        className="min-h-screen  bg-cover bg-center justify-center"
        style={{ backgroundImage: `url(${backgroundImage})` }}
      >
        <div className="h-screen ">
          <div className="min-h-screen items-center w-1/2 justify-center  my-auto flex flex-col">
            <div
              className=" py-8 px-4 sm:px-6 lg:px-8"
              style={{
                background: "linear-gradient(to right, #9B1B59, #6b2d98)",
              }}
            >
              <div className="max-w-md w-full space-y-8 ">
                <h2 className="mt-6 text-center text-3xl font-extrabold text-white">
                  Register
                </h2>
                {/* Role selection */}
                <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <label htmlFor="firstName" className="sr-only">
                        First Name
                      </label>
                      <input
                        id="firstName"
                        name="firstName"
                        type="text"
                        autoComplete="given-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="First Name"
                        value={formData.firstName}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="middlename" className="sr-only">
                        Middle Name
                      </label>
                      <input
                        id="middlename"
                        name="middlename"
                        type="text"
                        autoComplete="given-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Middle Name"
                        value={formData.middlename}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="lastName" className="sr-only">
                        Last Name
                      </label>
                      <input
                        id="lastName"
                        name="lastName"
                        type="text"
                        autoComplete="family-name"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Last Name"
                        value={formData.lastName}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="email" className="sr-only">
                        Email address
                      </label>
                      <input
                        id="email"
                        name="email"
                        type="email"
                        autoComplete="email"
                        required
                        className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                        placeholder="Email address"
                        value={formData.email}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label htmlFor="contactNumber" className="sr-only">
                        Contact Number
                      </label>
                      <input
                        id="contactNumber"
                        name="contactNumber"
                        type="text"
                        autoComplete="tel"
                        required
                        className="rounded-md align-top w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-black focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                        placeholder="Contact Number"
                        value={formData.contactNumber}
                        onChange={handleChange}
                      />
                    </div>
                    <div className="mt">
                      <button
                        type="button"
                        className="w-full py-2 px-4  bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 rounded-md text-sm text-white font-medium"
                        onClick={() => setConfirmationPopup(true)}
                      >
                        Pick Addresses
                      </button>
                    </div>
                  </div>
                  <div>
                    <button
                      type="submit"
                      onSubmit={handleSubmit}
                      className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                    >
                      Register
                    </button>
                  </div>
                </form>
              </div>
            </div>
            <footer className="text-center text-gray-500 text-sm py-4">
              All rights reserved &copy; {new Date().getFullYear()}{" "}
            </footer>
          </div>
        </div>
        {/* Address Popup */}
        {confirmationPopup && (
          <div class="fixed inset-0 flex items-center justify-center z-50 backdrop-blur confirm-dialog ">
            <div class="relative px-4 min-h-screen md:flex md:items-center md:justify-center">
              <div class=" opacity-25 w-full h-full absolute z-10 inset-0"></div>
              <div class="bg-white rounded-lg md:max-w-md md:mx-auto p-4 fixed inset-x-0 bottom-0 z-50 mb-4 mx-4 md:relative shadow-lg">
                <div class="md:flex items-center">
                  <div class="mt-4 md:mt-0 md:ml-6 text-center md:text-left">
                    <p class="font-bold">Success!</p>
                    <p class="text-sm text-gray-700 mt-1">
                      Location has successfully been set.
                    </p>
                    <div className="flex justify-end items-end mt-3">
                      <button
                        type="submit"
                        className="flex justify-end items-end py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-red-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 ml-5"
                        onClick={() => setConfirmationPopup(false)}
                      >
                        Close
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </>
  );
}

export default CreatingForm;
