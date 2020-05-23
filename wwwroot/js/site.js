// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function draw3dGraphPCA(pc1, pc2, pc3) {
    var data = null;
    var graph = null;

    function onclick(point) {
        console.log(point);
    }

    // Called when the Visualization API is loaded.
    function drawVisualization() {
        // create the data table.
        data = new vis.DataSet();

        // create the animation data
        var imax = 100;
        for (var i = 0; i < imax; i++) {
            x = pc1[i];
            y = pc2[i];
            z = pc3[i];
            var style = "#000000";

            data.add({ x: x, y: y, z: z, style: style });
        }

        // specify options
        var options = {
            width: "100%",
            height: "100%",
            style: "dot-color",
            showPerspective: true,
            showGrid: true,
            keepAspectRatio: true,
            verticalRatio: 1.0,
            showLegend: false,
            tooltip: {
                color: "red",
            },
            cameraPosition: {
                horizontal: -0.35,
                vertical: 0.22,
                distance: 1.8,
            },
            xLabel: "PC1",
            yLabel: "PC2",
            zLabel: "PC3"
        };

        // create our graph
        var container = document.getElementById("mygraph");
        graph = new vis.Graph3d(container, data, options);
        graph.on("click", onclick);
    }

    drawVisualization();
}