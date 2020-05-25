// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


/* FUNCTION FOR 3D GRAPHICS*/

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
        for (var i = 0; i < pc1.length; i++) {
            x = pc1[i];
            y = pc2[i];
            z = pc3[i];
            var style = "#0000CD";

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

function draw3dGraphKmeans(field1, field2, field3, clusters, field1Name, field2Name, field3Name) {
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
        for (var i = 0; i < field1.length; i++) {
            x = field1[i];
            y = field2[i];
            z = field3[i];
            var style = clusters[i] == 0 ? "#FFA500" : clusters[i] == 1 ? "#0000FF" : clusters[i] == 2 ? "#008000" : clusters[i] == 3 ? "#FFFF00" : clusters[i] == 4 ? "#4B0082" : "#000000";

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
            xLabel: field1Name,
            yLabel: field2Name,
            zLabel: field3Name
        };

        // create our graph
        var container = document.getElementById("mygraph");
        graph = new vis.Graph3d(container, data, options);
        graph.on("click", onclick);
    }

    drawVisualization();
}

function draw3dGraphPcaAndKmeans(pc1, pc2, pc3, clusters) {
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

        for (var i = 0; i < pc1.length; i++) {
            x = pc1[i];
            y = pc2[i];
            z = pc3[i];
            var style = clusters[i] == 0 ? "#FFA500" : clusters[i] == 1 ? "#0000FF" : clusters[i] == 2 ? "#008000" : clusters[i] == 3 ? "#FFFF00" : clusters[i] == 4 ? "#4B0082" : "#000000";

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

function drawBarChartProductResults(results, products) {
    var speedCanvas = document.getElementById("speedChart");
    var densityData = {
        label: 'Tasting mark',
        data: results
    };

    var barChart = new Chart(speedCanvas, {
        type: 'bar',
        data: {
            labels: products,
            datasets: [densityData]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        suggestedMax: 100
                    }
                }]
            }
        }
    });
}