function output = toCSV(data, path)
    fixations = data.fixations;
    len = length(fixations);
    fileId = fopen(path, 'w');
    fprintf(fileId, 'starting time, duration, x, y, z\n');
    for i = 1:len
        fprintf(fileId, '%.4f, %.4f, %.4f, %.4f, %.4f\n', fixations(1,i), fixations(2,i), fixations(3,i), fixations(4,i), fixations(5,i));
    end
    fclose(fileId);
    output = data;
end