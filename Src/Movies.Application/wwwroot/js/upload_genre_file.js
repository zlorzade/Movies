FilePond.registerPlugin(
    FilePondPluginFileValidateSize,
    FilePondPluginFileValidateType,
);

FilePond.setOptions({
    storeAsFile: true,
    instantUpload: false,
    acceptedFileTypes: ["application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"],
    credits: ('', ''),
    server: {
        process: {
            url: '/Genre/Add',
            method: 'POST',
            withCredentials: false,
            headers: {},
            timeout: 0,
            onload: null,
            onerror: (response) => console.log(response.data),
            ondata: null,
        },
    },
});
FilePond.setOptions(fa_ir);


const uploader = FilePond.create(document.querySelector('#file-upload'));

